namespace PooInterface.Core.Interfaces;

/// <summary>
/// Contrato para formatadores de texto.
/// Define o comportamento esperado: recebe texto e retorna texto formatado.
/// Abstração do "o que" fazer, não "como" fazer.
/// </summary>
public interface ITextFormatter
{
    /// <summary>
    /// Formata o texto de acordo com a estratégia específica de cada implementação.
    /// </summary>
    /// <param name="input">Texto a ser formatado</param>
    /// <returns>Texto formatado</returns>
    string Format(string input);
}
