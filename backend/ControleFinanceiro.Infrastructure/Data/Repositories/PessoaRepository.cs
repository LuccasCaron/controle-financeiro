using ControleFinanceiro.Domain.Entities;
using ControleFinanceiro.Domain.Repositories;
using Microsoft.EntityFrameworkCore;

namespace ControleFinanceiro.Infrastructure.Data.Repositories;

public class PessoaRepository : Repository<Pessoa>, IPessoaRepository
{
    #region Constructor

    public PessoaRepository(ApplicationDbContext context) : base(context)
    {
    }

    #endregion

    public async Task<Pessoa?> ObterPorIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .FirstOrDefaultAsync(p => p.Id == id, cancellationToken);
    }

    public IQueryable<Pessoa> ObterQueryable()
    {
        return _dbSet.AsQueryable();
    }
}
