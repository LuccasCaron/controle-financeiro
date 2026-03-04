using ControleFinanceiro.Domain.Entities;
using ControleFinanceiro.Domain.Enums;
using ControleFinanceiro.Domain.Repositories;
using ControleFinanceiro.Domain.ValueObjects;
using ControleFinanceiro.Infrastructure.Data.Repositories;
using Microsoft.EntityFrameworkCore;

namespace ControleFinanceiro.Infrastructure.Data;

public static class DatabaseInitializer
{
    public static async Task SeedAsync(ApplicationDbContext context)
    {
        if (await context.Pessoas.AnyAsync())
        {
            return;
        }

        var pessoaRepository = new PessoaRepository(context);
        var categoriaRepository = new CategoriaRepository(context);
        var transacaoRepository = new TransacaoRepository(context);

        var pessoa = new Pessoa(
            new Nome("João Silva"),
            new Cpf("11144477735"),
            new DateTime(1990, 1, 15, 0, 0, 0, DateTimeKind.Utc)
        );

        pessoaRepository.Adicionar(pessoa);
        await pessoaRepository.UnitOfWork.Commit();

        var categoriaSalario = new Categoria(
            new Nome("Salário"),
            new Descricao("Recebimento de salário mensal"),
            FinalidadeCategoria.Receita
        );

        var categoriaAlimentacao = new Categoria(
            new Nome("Alimentação"),
            new Descricao("Gastos com alimentação"),
            FinalidadeCategoria.Despesa
        );

        var categoriaTransporte = new Categoria(
            new Nome("Transporte"),
            new Descricao("Gastos com transporte"),
            FinalidadeCategoria.Despesa
        );

        var categoriaSaude = new Categoria(
            new Nome("Saúde"),
            new Descricao("Gastos com saúde"),
            FinalidadeCategoria.Despesa
        );

        categoriaRepository.Adicionar(categoriaSalario);
        categoriaRepository.Adicionar(categoriaAlimentacao);
        categoriaRepository.Adicionar(categoriaTransporte);
        categoriaRepository.Adicionar(categoriaSaude);
        await categoriaRepository.UnitOfWork.Commit();

        var transacaoSalario = new Transacao(
            pessoa,
            categoriaSalario,
            TipoTransacao.Receita,
            new Dinheiro(5000.00m),
            DateTime.UtcNow.AddDays(-5),
            new Descricao("Salário do mês")
        );

        transacaoRepository.Adicionar(transacaoSalario);
        await transacaoRepository.UnitOfWork.Commit();
    }
}
