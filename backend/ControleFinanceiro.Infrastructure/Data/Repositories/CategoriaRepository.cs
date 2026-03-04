using ControleFinanceiro.Domain.Entities;
using ControleFinanceiro.Domain.Repositories;
using Microsoft.EntityFrameworkCore;

namespace ControleFinanceiro.Infrastructure.Data.Repositories;

public class CategoriaRepository : Repository<Categoria>, ICategoriaRepository
{
    #region Constructor

    public CategoriaRepository(ApplicationDbContext context) : base(context)
    {
    }

    #endregion

    public async Task<Categoria?> ObterPorIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .FirstOrDefaultAsync(c => c.Id == id, cancellationToken);
    }

    public IQueryable<Categoria> ObterQueryable()
    {
        return _dbSet.AsQueryable();
    }
}
