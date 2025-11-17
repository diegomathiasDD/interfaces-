namespace PooInterface.Core.Formatters.WithInterface;

using PooInterface.Core.Interfaces;

/// <summary>
/// Implementação concreta: converte texto para maiúsculas.
/// </summary>
public sealed class UpperCaseFormatter : ITextFormatter
{
    public string Format(string input) => (input ?? string.Empty).ToUpperInvariant();
}

/// <summary>
/// Implementação concreta: converte texto para minúsculas.
/// </summary>
public sealed class LowerCaseFormatter : ITextFormatter
{
    public string Format(string input) => (input ?? string.Empty).ToLowerInvariant();
}

/// <summary>
/// Implementação concreta: converte texto para Title Case.
/// </summary>
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

/// <summary>
/// Implementação concreta: inverte a ordem dos caracteres.
/// </summary>
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

/// <summary>
/// Implementação concreta padrão: retorna texto sem alterações.
/// </summary>
public sealed class PlainFormatter : ITextFormatter
{
    public string Format(string input) => input ?? string.Empty;
}
