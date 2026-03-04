using ControleFinanceiro.Domain.Enums;

namespace ControleFinanceiro.Api.DTOs.Categoria;

/// <summary>
/// DTO para criação de categoria.
/// </summary>
public class CriarCategoriaDto
{
    /// <summary>
    /// Nome da categoria.
    /// </summary>
    public string Nome { get; set; } = string.Empty;

    /// <summary>
    /// Descrição da categoria (opcional).
    /// </summary>
    public string? Descricao { get; set; }

    /// <summary>
    /// Finalidade da categoria.
    /// </summary>
    public FinalidadeCategoria Finalidade { get; set; }
}
