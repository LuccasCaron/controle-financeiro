using ControleFinanceiro.Domain.Entities;
using ControleFinanceiro.Domain.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ControleFinanceiro.Infrastructure.Data.Mappings;

/// <summary>
/// Configuração de mapeamento da entidade Transacao usando Fluent API.
/// </summary>
public class TransacaoMapping : IEntityTypeConfiguration<Transacao>
{
    public void Configure(EntityTypeBuilder<Transacao> builder)
    {
        builder.ToTable("Transacoes");

        builder.HasKey(t => t.Id);

        builder.Property(t => t.PessoaId)
            .IsRequired();

        builder.Property(t => t.CategoriaId)
            .IsRequired();

        builder.Property(t => t.Tipo)
            .HasConversion<int>()
            .IsRequired();

        // Mapeamento do Value Object Dinheiro
        builder.OwnsOne(t => t.Valor, valor =>
        {
            valor.Property(v => v.Valor)
                .HasColumnName("Valor")
                .HasPrecision(18, 2)
                .IsRequired();
        });

        builder.Property(t => t.Data)
            .IsRequired();

        // Mapeamento do Value Object Descricao (opcional)
        builder.OwnsOne(t => t.Descricao, descricao =>
        {
            descricao.Property(d => d.Valor)
                .HasColumnName("Descricao")
                .HasMaxLength(500);
        });

        builder.Property(t => t.CriadoEm)
            .IsRequired();

        builder.Property(t => t.AtualizadoEm);

        // Relacionamentos
        builder.HasOne(t => t.Pessoa)
            .WithMany()
            .HasForeignKey(t => t.PessoaId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(t => t.Categoria)
            .WithMany()
            .HasForeignKey(t => t.CategoriaId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
