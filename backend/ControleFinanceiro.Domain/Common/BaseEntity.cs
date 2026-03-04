namespace ControleFinanceiro.Domain.Common;

/// <summary>
/// Classe base abstrata para todas as entidades do domínio.
/// Fornece identificador único (Guid) e controle de auditoria (datas de criação e atualização).
/// </summary>
public abstract class BaseEntity
{
    /// <summary>
    /// Identificador único da entidade.
    /// </summary>
    public Guid Id { get; protected set; }

    /// <summary>
    /// Data e hora de criação da entidade (UTC).
    /// </summary>
    public DateTime CriadoEm { get; protected set; }

    /// <summary>
    /// Data e hora da última atualização da entidade (UTC).
    /// Null se a entidade nunca foi atualizada.
    /// </summary>
    public DateTime? AtualizadoEm { get; protected set; }

    /// <summary>
    /// Construtor protegido que inicializa a entidade com um novo Guid e data de criação.
    /// </summary>
    protected BaseEntity()
    {
        Id = Guid.NewGuid();
        CriadoEm = DateTime.UtcNow;
    }

    /// <summary>
    /// Marca a entidade como atualizada, definindo a data de atualização.
    /// </summary>
    protected void MarcarComoAtualizado()
    {
        AtualizadoEm = DateTime.UtcNow;
    }
}
