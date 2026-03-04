using ControleFinanceiro.Domain.Common;
using ControleFinanceiro.Domain.Entities;

namespace ControleFinanceiro.Domain.Repositories;

/// <summary>
/// Interface para repositório de categorias.
/// </summary>
public interface ICategoriaRepository : IRepository<Categoria>
{
    /// <summary>
    /// Obtém uma categoria por seu identificador.
    /// </summary>
    /// <param name="id">Identificador da categoria.</param>
    /// <param name="cancellationToken">Token de cancelamento.</param>
    /// <returns>A categoria encontrada ou null se não existir.</returns>
    Task<Categoria?> ObterPorIdAsync(Guid id, CancellationToken cancellationToken = default);

    /// <summary>
    /// Obtém uma query para consultas de categorias.
    /// </summary>
    /// <returns>Query configurável para categorias.</returns>
    IQueryable<Categoria> ObterQueryable();
}
