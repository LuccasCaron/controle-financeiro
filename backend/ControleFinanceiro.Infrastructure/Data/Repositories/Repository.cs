using ControleFinanceiro.Domain.Common;
using Microsoft.EntityFrameworkCore;
using ControleFinanceiro.Infrastructure.Data;

namespace ControleFinanceiro.Infrastructure.Data.Repositories;

public class Repository<T> : IRepository<T> where T : BaseEntity
{
    #region Properties

    protected readonly ApplicationDbContext _context;
    protected readonly DbSet<T> _dbSet;

    #endregion

    #region Constructor

    public Repository(ApplicationDbContext context)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
        _dbSet = _context.Set<T>();
    }

    #endregion

    public IUnitOfWork UnitOfWork => _context;

    public void Adicionar(T entity)
    {
        _context.Add(entity);
    }

    public void Remover(T entity)
    {
        _context.Remove(entity);
    }
}
