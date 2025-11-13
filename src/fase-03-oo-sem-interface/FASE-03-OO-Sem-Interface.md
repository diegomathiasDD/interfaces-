# Fase 3 — OO sem interface

## Objetivo
Transformar a lógica procedural (Fase 2) em uma hierarquia orientada a objetos com polimorfismo, eliminando if/switch centralizado e distribuindo responsabilidade entre classes concretas especializadas.

## Design OO

### Hierarquia
```
TextFormatterBase (abstract)
├── UppercaseFormatter
├── LowercaseFormatter
├── TitleCaseFormatter
├── ReverseFormatter
└── DefaultFormatter
```

### Componentes

1. **TextFormatterBase** (classe abstrata)
   - Define contrato comum: método `Format(string text)` e propriedade `ModeName`.
   - Oferece método protegido `NormalizeInput()` para validação compartilhada.
   - Responsabilidade: estabelecer invariantes e comportamentos esperados de qualquer formatador.

2. **Formatadores concretos**
   - Cada um herda de `TextFormatterBase` e implementa `Format()`.
   - Cada classe tem uma única responsabilidade (e.g., `UppercaseFormatter` converte para maiúsculas).
   - Não contêm lógica de seleção; apenas aplicam sua transformação específica.

3. **FormatterFactory**
   - Registry baseado em Dictionary (sem switch/if).
   - Método `GetFormatter(string mode)` retorna a instância apropriada.
   - Se o modo for inválido, retorna `DefaultFormatter`.
   - Responsabilidade: centralizar seleção sem condicional procedural.

### Fluxo (Antes × Depois)

**Antes (Fase 2 — Procedural):**
```
modo = input
switch(modo) {
  case "upper": return Upper();
  case "lower": return Lower();
  case "title": return Title();
  case "reverse": return Reverse();
  default: return Default();
}
```

**Depois (Fase 3 — OO):**
```
formatter = Factory.GetFormatter(modo)
return formatter.Format(text)  // Polimorfismo: cada classe sabe como fazer sua transformação
```

## Melhorias

1. **Eliminação de if/switch procedural**
   - Decisão centralizada foi removida do fluxo principal.
   - Cada modo é representado por uma classe com responsabilidade única (SRP).

2. **Extensibilidade através de herança**
   - Novo modo? Crie uma nova classe que herde de `TextFormatterBase`.
   - Não é necessário alterar código existente (Open/Closed Principle).

3. **Testabilidade**
   - Cada formatador pode ser testado isoladamente (sem setup de switch/factory).
   - Testes de polimorfismo: passar qualquer formatador e validar o contrato comum.

4. **Redução de complexidade cognitiva**
   - Cada classe é pequena e legível (uma transformação = uma classe).
   - Novos membros da equipe entendem rapidamente: ver `UppercaseFormatter` é claro o que faz.

5. **Preparação para padrões avançados**
   - Estrutura pronta para interfaces (Fase 4), decoradores, composição, etc.

## Rigididades / Limitações

1. **Factory com Dictionary instanciado uma única vez**
   - Todas as instâncias de formatadores compartilham a mesma instância na factory.
   - Se um formatador precisar de estado mutável (future), será um problema.
   - Solução futura: criar formatadores dinamicamente ou usar injeção de dependência.

2. **Sem parametrização de formatadores**
   - `UppercaseFormatter` não aceita parâmetros (ex.: locale/encoding).
   - Se precisar customizar comportamento por modo, teria que criar múltiplas subclasses (ex.: `UppercaseFormatterPt`, `UppercaseFormatterEn`).
   - Solução: Strategy pattern com injeção de dependência (Fase 4/5).

3. **Factory ainda é um ponto de mudança**
   - Ao adicionar novo modo, é necessário alterar `FormatterFactory` (registrar nova instância).
   - Não é completamente isolado (acoplamento construtivo).
   - Solução: Auto-discovery via reflexão ou service locator (mais complexo, neste estágio overkill).

4. **Falta de composição dinâmica**
   - Não é possível combinar modos (ex.: "upper + reverse") sem criar novas classes.
   - Sem interfaces, não há contrato extensível para decoradores.

5. **Sem suporte a modos que dependem de contexto**
   - Ex.: formatar diferente se for email vs SMS (discutido na Fase 2).
   - Hierarquia rígida: cada modo é uma classe, sem forma natural de parametrização.

## Por que não usar switch nesta versão?

- **Switch é imperativo e centralizado**: concentra todas as decisões em um ponto, dificultando manutenção.
- **Acoplamento**: adicionar novo modo exige editar o switch (Open/Closed Principle violado).
- **Testes**: não há forma elegante de mockar ou substituir um ramo específico do switch.
- **Polimorfismo é melhor aqui**: delega responsabilidade a cada classe, permitindo especialização clara.

## Trade-offs vs Fase 2

| Aspecto | Fase 2 (Procedural) | Fase 3 (OO) |
|---------|---------------------|------------|
| Linhas de código | ~25 | ~100 (incluindo factory + classes) |
| Tempo de leitura | Rápido (função única) | Mais lento (visitar múltiplas classes) |
| Extensão | Editar função (switch) | Criar nova classe (herança) |
| Testabilidade | Média (testar função inteira) | Alta (cada classe isolada) |
| Acoplamento | Alto (todas decisões juntas) | Baixo (distribuído) |
| Escalabilidade | Fraca (switch cresce) | Forte (adicionar classes) |

## Próximos passos (Fase 4)

- Substituir `TextFormatterBase` por uma **interface**, permitindo múltiplas hierarquias.
- Adicionar **injeção de dependência** na factory.
- Permitir **composição e decoradores** sem alterar formatadores base.

---

Arquivo: `src/fase-03-oo-sem-interface/FASE-03-OO-Sem-Interface.md`
