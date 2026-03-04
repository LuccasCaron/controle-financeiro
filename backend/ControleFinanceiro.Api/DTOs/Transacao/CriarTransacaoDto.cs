using ControleFinanceiro.Domain.Enums;

namespace ControleFinanceiro.Api.DTOs.Transacao;

/// <summary>
/// DTO para criação de transação.
/// </summary>
public class CriarTransacaoDto
{
    /// <summary>
    /// Identificador da pessoa associada à transação.
    /// </summary>
    public Guid PessoaId { get; set; }

    /// <summary>
    /// Identificador da categoria associada à transação.
    /// </summary>
    public Guid CategoriaId { get; set; }

    /// <summary>
    /// Tipo da transação (Receita ou Despesa).
    /// </summary>
    public TipoTransacao Tipo { get; set; }

    /// <summary>
    /// Valor da transação.
    /// </summary>
    public decimal Valor { get; set; }

    /// <summary>
    /// Data da transação.
    /// </summary>
    public DateTime Data { get; set; }

    /// <summary>
    /// Descrição da transação (opcional).
    /// </summary>
    public string? Descricao { get; set; }
}
