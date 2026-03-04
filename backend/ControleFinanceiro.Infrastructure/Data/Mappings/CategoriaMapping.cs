using ControleFinanceiro.Domain.Entities;
using ControleFinanceiro.Domain.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ControleFinanceiro.Infrastructure.Data.Mappings;

/// <summary>
/// Configuração de mapeamento da entidade Categoria usando Fluent API.
/// </summary>
public class CategoriaMapping : IEntityTypeConfiguration<Categoria>
{
    public void Configure(EntityTypeBuilder<Categoria> builder)
    {
        builder.ToTable("Categorias");

        builder.HasKey(c => c.Id);

        // Mapeamento do Value Object Nome
        builder.OwnsOne(c => c.Nome, nome =>
        {
            nome.Property(n => n.Valor)
                .HasColumnName("Nome")
                .HasMaxLength(100)
                .IsRequired();
        });

        // Mapeamento do Value Object Descricao
        builder.OwnsOne(c => c.Descricao, descricao =>
        {
            descricao.Property(d => d.Valor)
                .HasColumnName("Descricao")
                .HasMaxLength(500);
        });

        builder.Property(c => c.Finalidade)
            .HasConversion<string>()
            .HasMaxLength(50)
            .IsRequired();

        builder.Property(c => c.CriadoEm)
            .IsRequired();

        builder.Property(c => c.AtualizadoEm);

        // Configuração de cascade delete para transações será feita no TransacaoMapping
    }
}
