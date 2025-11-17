namespace PooInterface.Tests.Doubles;

using PooInterface.Core.Interfaces;

/// <summary>
/// Dublê tipo Fake: implementação de teste para ITextFormatter.
/// Simula comportamento previsível sem I/O, permitindo testar o cliente em isolamento.
/// Útil para verificar como o cliente usa o formatador sem conhecer detalhes internos.
/// </summary>
public sealed class FakeFormatter : ITextFormatter
{
    /// <summary>
    /// Rastreia a última entrada recebida (útil para assertions no teste).
    /// </summary>
    public string LastReceivedInput { get; private set; } = string.Empty;

    /// <summary>
    /// Rastreia o número de vezes que foi chamado.
    /// </summary>
    public int CallCount { get; private set; }

    /// <summary>
    /// Comportamento previsível: envolve o input em marcadores [FAKE].
    /// </summary>
    public string Format(string input)
    {
        LastReceivedInput = input ?? string.Empty;
        CallCount++;
        return $"[FAKE]{input}[FAKE]";
    }

    /// <summary>
    /// Reseta o estado do dublê para testes múltiplos.
    /// </summary>
    public void Reset()
    {
        LastReceivedInput = string.Empty;
        CallCount = 0;
    }
}
