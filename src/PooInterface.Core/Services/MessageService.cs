namespace PooInterface.Core.Services;

using PooInterface.Core.Interfaces;

/// <summary>
/// Cliente que depende apenas do contrato (ITextFormatter).
/// Não conhece classes concretas; responsabilidade única é compor a mensagem usando o formatador injetado.
/// Exemplo de Dependency Injection via construtor.
/// </summary>
public sealed class MessageService
{
    private readonly ITextFormatter _formatter;

    /// <summary>
    /// Construtor: recebe a dependência (interface) injetada.
    /// Nunca instancia concretos; deixa isso a cargo do ponto de composição.
    /// </summary>
    public MessageService(ITextFormatter formatter)
    {
        _formatter = formatter ?? throw new ArgumentNullException(nameof(formatter));
    }

    /// <summary>
    /// Constrói uma mensagem formatada usando o formatador injetado.
    /// </summary>
    public string BuildMessage(string name)
    {
        var raw = $"Olá, {name}! Bem-vindo ao sistema.";
        return _formatter.Format(raw);
    }
}
