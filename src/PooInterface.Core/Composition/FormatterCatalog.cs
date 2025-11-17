namespace PooInterface.Core.Composition;

using PooInterface.Core.Interfaces;
using PooInterface.Core.Formatters.WithInterface;

/// <summary>
/// Ponto único de composição (catálogo / factory).
/// Responsabilidade: converter uma política (mode string) em uma implementação concreta de ITextFormatter.
/// Cliente e testes não precisam saber dos concretos; dependem apenas deste catálogo.
/// </summary>
public static class FormatterCatalog
{
    /// <summary>
    /// Resolve um formatador baseado no modo informado.
    /// Mantém toda a lógica de seleção centralizada.
    /// </summary>
    public static ITextFormatter Resolve(string? mode)
    {
        return (mode?.Trim().ToLowerInvariant()) switch
        {
            "upper" => new UpperCaseFormatter(),
            "lower" => new LowerCaseFormatter(),
            "title" => new TitleCaseFormatter(),
            "reverse" => new ReverseFormatter(),
            _ => new PlainFormatter() // padrão seguro
        };
    }

    /// <summary>
    /// Retorna todos os modos disponíveis (útil para help/documentação).
    /// </summary>
    public static IEnumerable<string> GetAvailableModes()
    {
        return new[] { "upper", "lower", "title", "reverse", "plain" };
    }
}
