namespace ControleFinanceiro.Domain.Exceptions;

/// <summary>
/// Exceção customizada para erros de domínio.
/// Deve ser lançada quando uma regra de negócio é violada.
/// </summary>
public class DomainException : Exception
{
    /// <summary>
    /// Inicializa uma nova instância da exceção de domínio com a mensagem especificada.
    /// </summary>
    /// <param name="message">Mensagem que descreve o erro.</param>
    public DomainException(string message) : base(message)
    {
    }

    /// <summary>
    /// Inicializa uma nova instância da exceção de domínio com a mensagem e a exceção interna especificadas.
    /// </summary>
    /// <param name="message">Mensagem que descreve o erro.</param>
    /// <param name="innerException">Exceção que é a causa da exceção atual.</param>
    public DomainException(string message, Exception innerException) : base(message, innerException)
    {
    }
}
