namespace ControleFinanceiro.Domain.Common;

/// <summary>
/// Interface genérica para repositórios.
/// Define operações básicas de persistência para entidades do domínio.
/// </summary>
/// <typeparam name="T">Tipo da entidade que deve herdar de BaseEntity.</typeparam>
public interface IRepository<T> where T : BaseEntity
{
    /// <summary>
    /// Obtém a instância do Unit of Work associada a este repositório.
    /// </summary>
    IUnitOfWork UnitOfWork { get; }

    /// <summary>
    /// Adiciona uma nova entidade ao contexto.
    /// </summary>
    /// <param name="entity">Entidade a ser adicionada.</param>
    void Adicionar(T entity);

    /// <summary>
    /// Remove uma entidade do contexto.
    /// </summary>
    /// <param name="entity">Entidade a ser removida.</param>
    void Remover(T entity);
}
