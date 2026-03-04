using ControleFinanceiro.Domain.Entities;
using ControleFinanceiro.Domain.Repositories;
using Microsoft.EntityFrameworkCore;

namespace ControleFinanceiro.Infrastructure.Data.Repositories;

public class TransacaoRepository : Repository<Transacao>, ITransacaoRepository
{
    #region Constructor

    public TransacaoRepository(ApplicationDbContext context) : base(context)
    {
    }

    #endregion

    public async Task<Transacao?> ObterPorIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .Include(t => t.Pessoa)
            .Include(t => t.Categoria)
            .FirstOrDefaultAsync(t => t.Id == id, cancellationToken);
    }

    public IQueryable<Transacao> ObterQueryable()
    {
        return _dbSet
            .Include(t => t.Pessoa)
            .Include(t => t.Categoria)
            .AsQueryable();
    }
}
