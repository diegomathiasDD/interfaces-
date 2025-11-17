using PooInterface.Core.Composition;
using PooInterface.Core.Interfaces;
using PooInterface.Core.Formatters.WithInterface;
using PooInterface.Core.Services;
using PooInterface.Tests.Doubles;
using Xunit;
using System;

namespace PooInterface.Tests.Phase4;

/// <summary>
/// Testes para Fase 4: Interface plugável e testável.
/// Demonstra composição, injeção de dependência e testes com dublês.
/// </summary>
public class Phase4InterfaceFormatterTests
{
    // Teste 1: Cliente com Fake (dublê) — sem I/O, sem concretos reais
    [Fact]
    public void MessageService_WithFakeFormatter_ShouldUseInjectedFormatter()
    {
        // Arrange
        var fake = new FakeFormatter();
        var service = new MessageService(fake);

        // Act
        var result = service.BuildMessage("Everton");

        // Assert
        Assert.Equal("[FAKE]Olá, Everton! Bem-vindo ao sistema.[FAKE]", result);
        Assert.Equal("Olá, Everton! Bem-vindo ao sistema.", fake.LastReceivedInput);
        Assert.Equal(1, fake.CallCount);
    }

    // Teste 2: Cliente com UpperCaseFormatter real
    [Fact]
    public void MessageService_WithUpperCaseFormatter_ShouldFormatUpperCase()
    {
        // Arrange
        ITextFormatter formatter = new UpperCaseFormatter();
        var service = new MessageService(formatter);

        // Act
        var result = service.BuildMessage("Diego");

        // Assert
        Assert.Equal("OLÁ, DIEGO! BEM-VINDO AO SISTEMA.", result);
    }

    // Teste 3: Cliente com LowerCaseFormatter real
    [Fact]
    public void MessageService_WithLowerCaseFormatter_ShouldFormatLowerCase()
    {
        // Arrange
        ITextFormatter formatter = new LowerCaseFormatter();
        var service = new MessageService(formatter);

        // Act
        var result = service.BuildMessage("João");

        // Assert
        Assert.Equal("olá, joão! bem-vindo ao sistema.", result);
    }

    // Teste 4: Cliente com TitleCaseFormatter real
    [Fact]
    public void MessageService_WithTitleCaseFormatter_ShouldFormatTitleCase()
    {
        // Arrange
        ITextFormatter formatter = new TitleCaseFormatter();
        var service = new MessageService(formatter);

        // Act
        var result = service.BuildMessage("Maria");

        // Assert
        Assert.Equal("Olá, Maria! Bem-vindo Ao Sistema.", result);
    }

    // Teste 5: Cliente com ReverseFormatter real
    [Fact]
    public void MessageService_WithReverseFormatter_ShouldReverseText()
    {
        // Arrange
        ITextFormatter formatter = new ReverseFormatter();
        var service = new MessageService(formatter);

        // Act
        var result = service.BuildMessage("Ana");

        // Assert
        // "Olá, Ana! Bem-vindo ao sistema." reversed (note: "vindo" é lowercase, não "Vindo")
        Assert.Equal(".ametsis oa odniv-meB !anA ,álO", result);
    }

    // Teste 6: Múltiplos fakes para validar múltiplas chamadas
    [Fact]
    public void MessageService_WithFakeFormatter_ShouldCallFormatterOncePerBuildMessage()
    {
        // Arrange
        var fake = new FakeFormatter();
        var service = new MessageService(fake);

        // Act
        service.BuildMessage("User1");
        service.BuildMessage("User2");

        // Assert
        Assert.Equal(2, fake.CallCount);
        Assert.Equal("Olá, User2! Bem-vindo ao sistema.", fake.LastReceivedInput);
    }

    // Teste 7: Catálogo resolve modos corretamente
    [Theory]
    [InlineData("upper")]
    [InlineData("lower")]
    [InlineData("title")]
    [InlineData("reverse")]
    [InlineData("plain")]
    [InlineData(null)]
    [InlineData("invalid")]
    public void FormatterCatalog_ResolvesValidAndInvalidModes(string mode)
    {
        // Act
        var formatter = FormatterCatalog.Resolve(mode);

        // Assert
        Assert.NotNull(formatter);
        Assert.IsAssignableFrom<ITextFormatter>(formatter);
    }

    // Teste 8: Catálogo retorna modo padrão para entrada inválida
    [Fact]
    public void FormatterCatalog_ResolveInvalidMode_ReturnsPainFormatter()
    {
        // Act
        var formatter = FormatterCatalog.Resolve("unknown");
        var result = formatter.Format("Test");

        // Assert (PlainFormatter retorna texto sem alterações)
        Assert.Equal("Test", result);
    }

    // Teste 9: Cliente rejeita null formatter
    [Fact]
    public void MessageService_Constructor_ThrowsWhenFormatterIsNull()
    {
        // Act & Assert
        Assert.Throws<ArgumentNullException>(() => new MessageService(null!));
    }

    // Teste 10: Catálogo oferece lista de modos disponíveis
    [Fact]
    public void FormatterCatalog_GetAvailableModes_ReturnsAllModes()
    {
        // Act
        var modes = FormatterCatalog.GetAvailableModes();

        // Assert
        Assert.Contains("upper", modes);
        Assert.Contains("lower", modes);
        Assert.Contains("title", modes);
        Assert.Contains("reverse", modes);
        Assert.Contains("plain", modes);
    }

    // Teste 11: Cenário de composição completa (sem FakeFormatter)
    [Fact]
    public void CompleteFlow_UpperMode_DeliverUpperCaseMessage()
    {
        // Arrange: composição em um único ponto
        var formatter = FormatterCatalog.Resolve("upper");
        var service = new MessageService(formatter);

        // Act
        var result = service.BuildMessage("Complete");

        // Assert
        Assert.Equal("OLÁ, COMPLETE! BEM-VINDO AO SISTEMA.", result);
    }

    // Teste 12: Cenário end-to-end com TitleCase
    [Fact]
    public void CompleteFlow_TitleMode_DeliverTitleCaseMessage()
    {
        // Arrange
        var formatter = FormatterCatalog.Resolve("title");
        var service = new MessageService(formatter);

        // Act
        var result = service.BuildMessage("System User");

        // Assert
        // TitleCase split por espaço: "Olá," "System" "User!" "Bem-vindo" "ao" "sistema."
        // Nota: hífen não é separador, então "Bem-vindo" vira "Bem-vindo" (B maiúsculo, resto minúsculo)
        Assert.Equal("Olá, System User! Bem-vindo Ao Sistema.", result);
    }
}
