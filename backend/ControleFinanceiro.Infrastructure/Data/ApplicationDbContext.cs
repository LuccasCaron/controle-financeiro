using ControleFinanceiro.Domain.Common;
using ControleFinanceiro.Domain.Entities;
using ControleFinanceiro.Domain.Exceptions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Npgsql;
using System.Reflection;

namespace ControleFinanceiro.Infrastructure.Data;

/// <summary>
/// Contexto do Entity Framework Core para acesso ao banco de dados.
/// </summary>
public class ApplicationDbContext : DbContext, IUnitOfWork
{
    /// <summary>
    /// Construtor que recebe as opções de configuração do DbContext.
    /// </summary>
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    /// <summary>
    /// DbSet para a entidade Pessoa.
    /// </summary>
    public DbSet<Pessoa> Pessoas => Set<Pessoa>();

    /// <summary>
    /// DbSet para a entidade Categoria.
    /// </summary>
    public DbSet<Categoria> Categorias => Set<Categoria>();

    /// <summary>
    /// DbSet para a entidade Transacao.
    /// </summary>
    public DbSet<Transacao> Transacoes => Set<Transacao>();

    /// <summary>
    /// Configura o modelo de dados usando Fluent API.
    /// </summary>
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Aplica as configurações de mapeamento
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(ApplicationDbContext).Assembly);

        // Seed automático
        SeedData(modelBuilder);
    }

    /// <summary>
    /// Popula o banco de dados com dados iniciais.
    /// </summary>
    private void SeedData(ModelBuilder modelBuilder)
    {
        // Seed será implementado nos mappings
    }

    /// <summary>
    /// Salva todas as alterações pendentes no contexto e retorna se houve alterações salvas.
    /// Atualiza automaticamente a data de atualização para entidades modificadas.
    /// </summary>
    /// <returns>True se houve alterações salvas, False caso contrário.</returns>
    public async Task<bool> Commit()
    {
        foreach (var entry in ChangeTracker.Entries()
            .Where(entry => entry.Entity is BaseEntity))
        {
            var entity = (BaseEntity)entry.Entity;

            if (entry.State == EntityState.Modified)
            {
                var property = typeof(BaseEntity).GetProperty(nameof(BaseEntity.AtualizadoEm), 
                    BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
                property?.SetValue(entity, DateTime.UtcNow);
            }
        }

        try
        {
            return await base.SaveChangesAsync() > 0;
        }
        catch (DbUpdateConcurrencyException ex)
        {
            throw new DomainException(
                "O registro foi modificado por outro usuário. Por favor, recarregue os dados e tente novamente.",
                ex);
        }
        catch (DbUpdateException ex)
        {
            if (ex.InnerException is PostgresException postgresEx)
            {
                if (postgresEx.SqlState == "23505")
                {
                    var constraintName = postgresEx.ConstraintName ?? "constraint";
                    var mensagem = constraintName.ToLower().Contains("cpf")
                        ? "Já existe uma pessoa cadastrada com este CPF."
                        : constraintName.ToLower().Contains("nome")
                            ? "Já existe uma categoria com este nome."
                            : "Violação de constraint única. O registro já existe.";

                    throw new DomainException(mensagem, ex);
                }

                if (postgresEx.SqlState == "23503")
                {
                    throw new DomainException(
                        "Não é possível realizar esta operação. O registro está sendo referenciado por outros dados.",
                        ex);
                }

                if (postgresEx.SqlState == "23502")
                {
                    throw new DomainException(
                        "Erro ao salvar: um campo obrigatório não foi preenchido.",
                        ex);
                }
            }

            throw new DomainException(
                "Erro ao salvar as alterações no banco de dados. Tente novamente mais tarde.",
                ex);
        }
    }
}
