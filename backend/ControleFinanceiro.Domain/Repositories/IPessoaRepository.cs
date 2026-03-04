using ControleFinanceiro.Domain.Common;
using ControleFinanceiro.Domain.Entities;

namespace ControleFinanceiro.Domain.Repositories;

/// <summary>
/// Interface para repositório de pessoas.
/// </summary>
public interface IPessoaRepository : IRepository<Pessoa>
{
    /// <summary>
    /// Obtém uma pessoa por seu identificador.
    /// </summary>
    /// <param name="id">Identificador da pessoa.</param>
    /// <param name="cancellationToken">Token de cancelamento.</param>
    /// <returns>A pessoa encontrada ou null se não existir.</returns>
    Task<Pessoa?> ObterPorIdAsync(Guid id, CancellationToken cancellationToken = default);

    /// <summary>
    /// Obtém uma query para consultas de pessoas.
    /// </summary>
    /// <returns>Query configurável para pessoas.</returns>
    IQueryable<Pessoa> ObterQueryable();
}
