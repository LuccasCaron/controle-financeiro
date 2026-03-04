namespace ControleFinanceiro.Domain.Common;

/// <summary>
/// Interface para o padrão Unit of Work.
/// Responsável por gerenciar transações e persistência de dados.
/// </summary>
public interface IUnitOfWork
{
    /// <summary>
    /// Salva todas as alterações pendentes no contexto e retorna se houve alterações salvas.
    /// </summary>
    /// <returns>True se houve alterações salvas, False caso contrário.</returns>
    Task<bool> Commit();
}
