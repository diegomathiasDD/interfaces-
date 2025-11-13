using PooInterface.Core.Formatters;
using Xunit;

namespace PooInterface.Tests;

public class FormatterTests
{
    [Fact]
    public void Procedural_TitleCase_Works()
    {
        var input = "hello test";
        var outp = FormatterProcedural.Format(input, FormatterProcedural.Mode.TitleCase);
        Assert.Equal("Hello Test", outp);
    }

    [Fact]
    public void OO_TitleCase_Works()
    {
        var f = new TitleCaseFormatter();
        Assert.Equal("Hello Test", f.Format("hello test"));
    }

    [Fact]
    public void Interface_TitleCase_Works()
    {
        var f = new InterfaceTitleCaseFormatter();
        Assert.Equal("Hello Test", f.Format("hello test"));
    }
}

/// <summary>
/// Testes para Fase 3: OO sem interface.
/// Valida polimorfismo, factory, e cenários de fronteira.
/// </summary>
public class Phase3OOFormatterTests
{
    // Cenário 1: Valor/entrada mínima (string vazia)
    [Fact]
    public void Scenario1_EmptyString_WithUpperFormatter_ReturnsEmpty()
    {
        var formatter = new UpperFormatter();
        var result = formatter.Format("");
        Assert.Equal("", result);
    }

    // Cenário 2: Valor/entrada máxima/limite (string grande)
    [Fact]
    public void Scenario2_LargeString_WithLowercaseFormatter_CompletesWithoutException()
    {
        var largeString = new string('A', 100_000);
        var formatter = new LowerFormatter();
        var result = formatter.Format(largeString);
        
        Assert.NotNull(result);
        Assert.Equal(100_000, result.Length);
        Assert.Equal(new string('a', 100_000), result);
    }

    // Cenário 3: Modo inválido / factory retorna default (plain)
    [Fact]
    public void Scenario3_InvalidMode_FactoryReturnsPlainFormatter()
    {
        var formatter = FormatterOOFactory.GetFormatter("unknown_mode");
        var result = formatter.Format("Hello World");
        
        // Plain formatter retorna a entrada sem alteração
        Assert.Equal("Hello World", result);
    }

    // Cenário 4: Combinação com espaços e pontuação (title case)
    [Fact]
    public void Scenario4_TextWithHyphensAndSpaces_TitleCaseHandlesCorrectly()
    {
        var formatter = FormatterOOFactory.GetFormatter("title");
        var result = formatter.Format("  hello-world  ");
        
        // Title case considera separador por espaços apenas (hífen não é separador)
        Assert.Equal("  Hello-world  ", result);
    }

    // Cenário 5: Caso comum representativo (texto com acentos)
    [Fact]
    public void Scenario5_TextWithAccents_LowercasePreservesUnicode()
    {
        var formatter = FormatterOOFactory.GetFormatter("lower");
        var result = formatter.Format("Olá Mundo");
        
        Assert.Equal("olá mundo", result);
    }

    // Teste de polimorfismo: múltiplos formatadores respondem ao contrato comum
    [Theory]
    [InlineData("upper", "HELLO")]
    [InlineData("lower", "hello")]
    [InlineData("title", "Hello")]
    [InlineData("reverse", "olleh")]
    [InlineData("plain", "hello")]
    public void Polymorphism_AllFormattersImplementContract(string mode, string expectedResult)
    {
        var formatter = FormatterOOFactory.GetFormatter(mode);
        var result = formatter.Format("hello");
        
        Assert.Equal(expectedResult, result);
        Assert.NotNull(formatter);
    }

    // Teste de factory: null mode retorna plain
    [Fact]
    public void Factory_NullMode_ReturnsPlainFormatter()
    {
        string? nullMode = null;
        var formatter = FormatterOOFactory.GetFormatter(nullMode!);
        var result = formatter.Format("test");
        Assert.Equal("test", result);
    }

    // Teste de factory: modos disponíveis
    [Fact]
    public void Factory_GetAvailableModes_ReturnsAllModes()
    {
        var modes = FormatterOOFactory.GetAvailableModes();
        Assert.Contains("upper", modes);
        Assert.Contains("lower", modes);
        Assert.Contains("title", modes);
        Assert.Contains("reverse", modes);
        Assert.Contains("plain", modes);
    }

    // Teste de reverse
    [Fact]
    public void ReverseFormatter_ReversesTextCorrectly()
    {
        var formatter = new ReverseFormatter();
        var result = formatter.Format("hello");
        Assert.Equal("olleh", result);
    }
}