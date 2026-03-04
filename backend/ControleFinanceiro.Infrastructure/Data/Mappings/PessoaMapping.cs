using ControleFinanceiro.Domain.Entities;
using ControleFinanceiro.Domain.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ControleFinanceiro.Infrastructure.Data.Mappings;

/// <summary>
/// Configuração de mapeamento da entidade Pessoa usando Fluent API.
/// </summary>
public class PessoaMapping : IEntityTypeConfiguration<Pessoa>
{
    public void Configure(EntityTypeBuilder<Pessoa> builder)
    {
        builder.ToTable("Pessoas");

        builder.HasKey(p => p.Id);

        // Mapeamento do Value Object Nome
        builder.OwnsOne(p => p.Nome, nome =>
        {
            nome.Property(n => n.Valor)
                .HasColumnName("Nome")
                .HasMaxLength(100)
                .IsRequired();
        });

        // Mapeamento do Value Object CPF
        builder.OwnsOne(p => p.Cpf, cpf =>
        {
            cpf.Property(c => c.Valor)
                .HasColumnName("Cpf")
                .HasMaxLength(11)
                .IsRequired();

            cpf.HasIndex(c => c.Valor)
                .IsUnique();
        });

        builder.Property(p => p.DataNascimento)
            .IsRequired();

        // Propriedades calculadas não são mapeadas (Idade, EMenorDeIdade)

        builder.Property(p => p.CriadoEm)
            .IsRequired();

        builder.Property(p => p.AtualizadoEm);

        // Configuração de cascade delete para transações será feita no TransacaoMapping
    }
}
