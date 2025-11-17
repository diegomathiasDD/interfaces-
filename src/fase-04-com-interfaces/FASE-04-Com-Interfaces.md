# Fase 4 — Interface plugável e testável

## Objetivo
Evoluir a Fase 3 introduzindo um **contrato explícito** (interface) para o comportamento variável, um **ponto único de composição**, e demonstrar como **testar com dublês** (fakes/stubs) sem I/O, tornando o cliente independente de implementações concretas.

## Estrutura da Solução

### 1. Contrato (Interface)

```csharp
namespace PooInterface.Core.Interfaces;

/// <summary>
/// Contrato para formatadores de texto.
/// Define o "o que" (comportamento esperado), não o "como" (implementação).
/// </summary>
public interface ITextFormatter
{
    string Format(string input);
}
```

**Propósito:**
- Abstração do comportamento variável.
- Cliente não conhece concretos; depende apenas dessa interface.
- Permite múltiplas implementações plugáveis.

---

### 2. Implementações Concretas

```csharp
namespace PooInterface.Core.Formatters.WithInterface;

using PooInterface.Core.Interfaces;

public sealed class UpperCaseFormatter : ITextFormatter
{
    public string Format(string input) => (input ?? string.Empty).ToUpperInvariant();
}

public sealed class LowerCaseFormatter : ITextFormatter
{
    public string Format(string input) => (input ?? string.Empty).ToLowerInvariant();
}

public sealed class TitleCaseFormatter : ITextFormatter
{
    public string Format(string input)
    {
        if (string.IsNullOrWhiteSpace(input)) return string.Empty;
        var words = input.Split(' ');
        for (int i = 0; i < words.Length; i++)
        {
            if (words[i].Length == 0) continue;
            var first = char.ToUpperInvariant(words[i][0]);
            var rest = words[i].Substring(1).ToLowerInvariant();
            words[i] = first + rest;
        }
        return string.Join(" ", words);
    }
}

public sealed class ReverseFormatter : ITextFormatter
{
    public string Format(string input)
    {
        var normalized = input ?? string.Empty;
        var arr = normalized.ToCharArray();
        System.Array.Reverse(arr);
        return new string(arr);
    }
}

public sealed class PlainFormatter : ITextFormatter
{
    public string Format(string input) => input ?? string.Empty;
}
```

**Características:**
- Cada classe é selada (`sealed`): não herdam, implementam apenas.
- Responsabilidade única: uma transformação por classe.
- Implementam o contrato comum (`ITextFormatter`).

---

### 3. Cliente Dependente de Interface

```csharp
namespace PooInterface.Core.Services;

using PooInterface.Core.Interfaces;

public sealed class MessageService
{
    private readonly ITextFormatter _formatter;

    /// <summary>
    /// Injeção de dependência via construtor.
    /// Cliente não instancia concretos; recebe a interface.
    /// </summary>
    public MessageService(ITextFormatter formatter)
    {
        _formatter = formatter ?? throw new ArgumentNullException(nameof(formatter));
    }

    public string BuildMessage(string name)
    {
        var raw = $"Olá, {name}! Bem-vindo ao sistema.";
        return _formatter.Format(raw);
    }
}
```

**Princípios:**
- **Inversão de Controle (IoC):** o cliente recebe a dependência, não cria.
- **Segregação de Interface (ISP):** cliente depende apenas de `ITextFormatter`, nada mais.
- **Independência:** cliente não muda ao adicionar novos formatadores.

---

### 4. Ponto Único de Composição (Catálogo/Factory)

```csharp
namespace PooInterface.Core.Composition;

using PooInterface.Core.Interfaces;
using PooInterface.Core.Formatters.WithInterface;

public static class FormatterCatalog
{
    public static ITextFormatter Resolve(string? mode)
    {
        return (mode?.Trim().ToLowerInvariant()) switch
        {
            "upper" => new UpperCaseFormatter(),
            "lower" => new LowerCaseFormatter(),
            "title" => new TitleCaseFormatter(),
            "reverse" => new ReverseFormatter(),
            _ => new PlainFormatter()
        };
    }

    public static IEnumerable<string> GetAvailableModes()
    {
        return new[] { "upper", "lower", "title", "reverse", "plain" };
    }
}
```

**Propósito:**
- Local **único** que decide qual implementação criar.
- Sem novo espalhado no cliente.
- Sem switch no código de negócio.
- Reutilizável em toda a aplicação.

---

### 5. Teste com Dublê (Fake)

```csharp
namespace PooInterface.Tests.Doubles;

using PooInterface.Core.Interfaces;

public sealed class FakeFormatter : ITextFormatter
{
    public string LastReceivedInput { get; private set; } = string.Empty;
    public int CallCount { get; private set; }

    public string Format(string input)
    {
        LastReceivedInput = input ?? string.Empty;
        CallCount++;
        return $"[FAKE]{input}[FAKE]";
    }

    public void Reset()
    {
        LastReceivedInput = string.Empty;
        CallCount = 0;
    }
}
```

**Vantagens:**
- Implementa a mesma interface.
- Sem I/O real.
- Comportamento previsível.
- Permite rastrear interações (espionagem).
- Rápido e determinístico.

---

### 6. Testes com Dublês (Exemplos)

```csharp
public class MessageServiceTests
{
    // Teste com Fake (sem I/O, sem concretos reais)
    [Fact]
    public void BuildMessage_DeveUsarFormatterInjetado()
    {
        // Arrange
        var fake = new FakeFormatter();
        var service = new MessageService(fake);

        // Act
        var result = service.BuildMessage("Everton");

        // Assert
        Assert.Equal("Olá, Everton! Bem-vindo ao sistema.", fake.LastReceivedInput);
        Assert.Equal("[FAKE]Olá, Everton! Bem-vindo ao sistema.[FAKE]", result);
        Assert.Equal(1, fake.CallCount);
    }

    // Teste end-to-end com implementação real
    [Fact]
    public void BuildMessage_ComFormatterReal_DeveFormatarCorretamente()
    {
        // Arrange
        ITextFormatter formatter = new UpperCaseFormatter();
        var service = new MessageService(formatter);

        // Act
        var result = service.BuildMessage("Diego");

        // Assert
        Assert.Equal("OLÁ, DIEGO! BEM-VINDO AO SISTEMA.", result);
    }

    // Teste de composição via catálogo
    [Fact]
    public void CatalogResolve_DeveRetornarFormatterValido()
    {
        // Act
        var formatter = FormatterCatalog.Resolve("title");
        var result = formatter.Format("hello world");

        // Assert
        Assert.NotNull(formatter);
        Assert.IsAssignableFrom<ITextFormatter>(formatter);
        Assert.Equal("Hello World", result);
    }
}
```

---

## Fluxo de Uso Completo

```csharp
// Ponto de entrada (Program.cs ou setup da aplicação)
public class Program
{
    public static void Main(string[] args)
    {
        // Política: modo vem da configuração/usuário
        var mode = "upper"; // ex.: de appsettings.json ou entrada do usuário

        // Composição em um único ponto
        ITextFormatter formatter = FormatterCatalog.Resolve(mode);

        // Cliente recebe a interface
        var service = new MessageService(formatter);

        // Uso
        var message = service.BuildMessage("João");
        Console.WriteLine(message); // "OLÁ, JOÃO! BEM-VINDO AO SISTEMA."
    }
}
```

---

## Melhorias vs Fase 3

| Aspecto | Fase 3 (OO, sem interface) | Fase 4 (Com interface) |
|---------|---------------------------|----------------------|
| Contrato | Classe base concreta | Interface abstrata |
| Acoplamento | Cliente usa `FormatterBase` | Cliente usa `ITextFormatter` |
| Extensão | Herdar classe base | Implementar interface |
| Testabilidade | Difícil criar dublês (herança rígida) | Fácil criar fakes/stubs |
| Composição | Factory com Dictionary | Factory retorna interface |
| Isolamento | Parcial (base concreta) | Total (interface) |

---

## Princípios Aplicados

1. **Dependency Inversion Principle (DIP):**
   - Cliente depende de abstração (`ITextFormatter`), não de concretos.

2. **Segregation Interface Principle (ISP):**
   - Interface é mínima: apenas `Format()`.
   - Cliente não conhece métodos extras.

3. **Single Responsibility Principle (SRP):**
   - Cada formatador tem uma responsabilidade.
   - `MessageService` foca em compor mensagem, não em selecionar formatador.

4. **Open/Closed Principle (OCP):**
   - Adicionar novo formatador: cria classe, registra em catálogo.
   - Sem modificar cliente ou existentes.

5. **Composition over Inheritance:**
   - Cliente compõe (`new MessageService(formatter)`), não herda.

---

## Por que Interfaces vs Classes Base?

- **Interfaces** permitem múltiplas implementações sem hierarquia rígida.
- **Dublês** são triviais: basta implementar a interface.
- **Testes** isolam o cliente da implementação real.
- **Futuro:** suporta decoradores, proxies, composição avançada.

---

## Próximos Passos (Fase 5+)

- **Fase 5:** Essenciais de interfaces em C# (contravarância, covariância, default implementations).
- **Fase 6:** ISP na prática (quebrar interfaces grandes).
- **Fase 7+:** Repository pattern com interfaces (InMemory, CSV, JSON).

---

Arquivo: `src/fase-04-com-interfaces/FASE-04-Com-Interfaces.md`
