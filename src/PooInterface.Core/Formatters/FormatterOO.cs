using System;
using System.Collections.Generic;

namespace PooInterface.Core.Formatters;

/// <summary>
/// Fase 3 — OO sem interface: hierarquia com polimorfismo
/// Base abstrata com implementações concretas.
/// </summary>
public abstract class FormatterBase
{
    public abstract string Format(string input);
}

/// <summary>
/// Formatador padrão: retorna o texto sem alterações.
/// </summary>
public sealed class PlainFormatter : FormatterBase
{
    public override string Format(string input) => input ?? string.Empty;
}

/// <summary>
/// Converte texto para Title Case.
/// </summary>
public sealed class TitleCaseFormatter : FormatterBase
{
    public override string Format(string input)
    {
        if (string.IsNullOrWhiteSpace(input)) return string.Empty;
        var parts = input.Split(' ');
        for (int i = 0; i < parts.Length; i++)
        {
            var p = parts[i];
            if (p.Length == 0) continue;
            parts[i] = char.ToUpperInvariant(p[0]) + (p.Length > 1 ? p.Substring(1).ToLowerInvariant() : string.Empty);
        }
        return string.Join(' ', parts);
    }
}

/// <summary>
/// Converte texto para maiúsculas.
/// </summary>
public sealed class UpperFormatter : FormatterBase
{
    public override string Format(string input) => (input ?? string.Empty).ToUpperInvariant();
}

/// <summary>
/// Converte texto para minúsculas.
/// </summary>
public sealed class LowerFormatter : FormatterBase
{
    public override string Format(string input) => (input ?? string.Empty).ToLowerInvariant();
}

/// <summary>
/// Inverte a ordem dos caracteres.
/// </summary>
public sealed class ReverseFormatter : FormatterBase
{
    public override string Format(string input)
    {
        var normalized = input ?? string.Empty;
        var arr = normalized.ToCharArray();
        System.Array.Reverse(arr);
        return new string(arr);
    }
}

/// <summary>
/// Factory para seleção de formatadores sem switch/if.
/// Implementa Registry pattern com Dictionary.
/// Responsabilidade: mapear modos para instâncias de formatadores.
/// </summary>
public static class FormatterOOFactory
{
    private static readonly Dictionary<string, FormatterBase> _formatters = new()
    {
        { "upper", new UpperFormatter() },
        { "lower", new LowerFormatter() },
        { "title", new TitleCaseFormatter() },
        { "reverse", new ReverseFormatter() },
        { "plain", new PlainFormatter() }
    };

    public static FormatterBase GetFormatter(string mode)
    {
        var normalizedMode = (mode ?? string.Empty).Trim().ToLowerInvariant();
        return _formatters.TryGetValue(normalizedMode, out var formatter)
            ? formatter
            : _formatters["plain"];
    }

    public static IEnumerable<string> GetAvailableModes()
    {
        return _formatters.Keys;
    }
}