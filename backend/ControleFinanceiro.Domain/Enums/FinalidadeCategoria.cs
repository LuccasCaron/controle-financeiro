namespace ControleFinanceiro.Domain.Enums;

/// <summary>
/// Enum que representa a finalidade de uma categoria.
/// Define se a categoria é usada para receitas, despesas ou ambas.
/// </summary>
public enum FinalidadeCategoria
{
    /// <summary>
    /// Categoria exclusiva para receitas.
    /// </summary>
    Receita = 1,

    /// <summary>
    /// Categoria exclusiva para despesas.
    /// </summary>
    Despesa = 2,

    /// <summary>
    /// Categoria que pode ser usada tanto para receitas quanto para despesas.
    /// </summary>
    Ambas = 3
}
