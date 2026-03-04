namespace ControleFinanceiro.Domain.Enums;

/// <summary>
/// Enum que representa o tipo de transação financeira.
/// </summary>
public enum TipoTransacao
{
    /// <summary>
    /// Transação de receita (entrada de dinheiro).
    /// </summary>
    Receita = 1,

    /// <summary>
    /// Transação de despesa (saída de dinheiro).
    /// </summary>
    Despesa = 2
}
