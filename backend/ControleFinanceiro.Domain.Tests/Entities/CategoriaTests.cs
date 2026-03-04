using ControleFinanceiro.Domain.Entities;
using ControleFinanceiro.Domain.Enums;
using ControleFinanceiro.Domain.Exceptions;
using ControleFinanceiro.Domain.ValueObjects;
using FluentAssertions;

namespace ControleFinanceiro.Domain.Tests.Entities;

public class CategoriaTests
{
    #region Properties
    #endregion

    #region Constructor
    #endregion

    [Fact]
    public void Deve_Criar_Categoria_Com_Dados_Validos()
    {
        var nome = new Nome("Alimentação");
        var descricao = new Descricao("Gastos com alimentação");
        var finalidade = FinalidadeCategoria.Despesa;

        var categoria = new Categoria(nome, descricao, finalidade);

        categoria.Nome.Should().Be(nome);
        categoria.Descricao.Should().Be(descricao);
        categoria.Finalidade.Should().Be(finalidade);
    }

    [Fact]
    public void Deve_Atualizar_Nome()
    {
        var nome = new Nome("Alimentação");
        var descricao = new Descricao("Gastos com alimentação");
        var finalidade = FinalidadeCategoria.Despesa;
        var categoria = new Categoria(nome, descricao, finalidade);

        var novoNome = new Nome("Comida");
        categoria.AtualizarNome(novoNome);

        categoria.Nome.Should().Be(novoNome);
    }

    [Fact]
    public void Deve_Atualizar_Descricao()
    {
        var nome = new Nome("Alimentação");
        var descricao = new Descricao("Gastos com alimentação");
        var finalidade = FinalidadeCategoria.Despesa;
        var categoria = new Categoria(nome, descricao, finalidade);

        var novaDescricao = new Descricao("Nova descrição");
        categoria.AtualizarDescricao(novaDescricao);

        categoria.Descricao.Should().Be(novaDescricao);
    }

    [Fact]
    public void Deve_Atualizar_Finalidade()
    {
        var nome = new Nome("Alimentação");
        var descricao = new Descricao("Gastos com alimentação");
        var finalidade = FinalidadeCategoria.Despesa;
        var categoria = new Categoria(nome, descricao, finalidade);

        categoria.AtualizarFinalidade(FinalidadeCategoria.Ambas);

        categoria.Finalidade.Should().Be(FinalidadeCategoria.Ambas);
    }

    [Fact]
    public void Deve_Permitir_Receita_Para_Categoria_Receita()
    {
        var nome = new Nome("Salário");
        var descricao = new Descricao("Recebimento de salário");
        var finalidade = FinalidadeCategoria.Receita;
        var categoria = new Categoria(nome, descricao, finalidade);

        Action act = () => categoria.ValidarTipoTransacaoPermitido(TipoTransacao.Receita);

        act.Should().NotThrow();
    }

    [Fact]
    public void Deve_Lancar_Excecao_Quando_Categoria_Receita_Usada_Para_Despesa()
    {
        var nome = new Nome("Salário");
        var descricao = new Descricao("Recebimento de salário");
        var finalidade = FinalidadeCategoria.Receita;
        var categoria = new Categoria(nome, descricao, finalidade);

        Action act = () => categoria.ValidarTipoTransacaoPermitido(TipoTransacao.Despesa);

        act.Should().Throw<DomainException>()
            .Which.Message.Should().Contain("é exclusiva para receitas e não pode ser usada para despesa");
    }

    [Fact]
    public void Deve_Permitir_Despesa_Para_Categoria_Despesa()
    {
        var nome = new Nome("Alimentação");
        var descricao = new Descricao("Gastos com alimentação");
        var finalidade = FinalidadeCategoria.Despesa;
        var categoria = new Categoria(nome, descricao, finalidade);

        Action act = () => categoria.ValidarTipoTransacaoPermitido(TipoTransacao.Despesa);

        act.Should().NotThrow();
    }

    [Fact]
    public void Deve_Lancar_Excecao_Quando_Categoria_Despesa_Usada_Para_Receita()
    {
        var nome = new Nome("Alimentação");
        var descricao = new Descricao("Gastos com alimentação");
        var finalidade = FinalidadeCategoria.Despesa;
        var categoria = new Categoria(nome, descricao, finalidade);

        Action act = () => categoria.ValidarTipoTransacaoPermitido(TipoTransacao.Receita);

        act.Should().Throw<DomainException>()
            .Which.Message.Should().Contain("é exclusiva para despesas e não pode ser usada para receita");
    }

    [Fact]
    public void Deve_Permitir_Receita_Para_Categoria_Ambas()
    {
        var nome = new Nome("Geral");
        var descricao = new Descricao("Categoria geral");
        var finalidade = FinalidadeCategoria.Ambas;
        var categoria = new Categoria(nome, descricao, finalidade);

        Action act = () => categoria.ValidarTipoTransacaoPermitido(TipoTransacao.Receita);

        act.Should().NotThrow();
    }

    [Fact]
    public void Deve_Permitir_Despesa_Para_Categoria_Ambas()
    {
        var nome = new Nome("Geral");
        var descricao = new Descricao("Categoria geral");
        var finalidade = FinalidadeCategoria.Ambas;
        var categoria = new Categoria(nome, descricao, finalidade);

        Action act = () => categoria.ValidarTipoTransacaoPermitido(TipoTransacao.Despesa);

        act.Should().NotThrow();
    }
}
