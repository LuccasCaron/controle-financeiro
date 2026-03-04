using ControleFinanceiro.Domain.Common;
using ControleFinanceiro.Domain.Entities;

namespace ControleFinanceiro.Domain.Repositories;

/// <summary>
/// Interface para repositório de transações.
/// </summary>
public interface ITransacaoRepository : IRepository<Transacao>
{
    /// <summary>
    /// Obtém uma transação por seu identificador.
    /// </summary>
    /// <param name="id">Identificador da transação.</param>
    /// <param name="cancellationToken">Token de cancelamento.</param>
    /// <returns>A transação encontrada ou null se não existir.</returns>
    Task<Transacao?> ObterPorIdAsync(Guid id, CancellationToken cancellationToken = default);

    /// <summary>
    /// Obtém uma query para consultas de transações.
    /// </summary>
    /// <returns>Query configurável para transações.</returns>
    IQueryable<Transacao> ObterQueryable();
}
