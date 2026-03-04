using ControleFinanceiro.Domain.Entities;
using ControleFinanceiro.Domain.Enums;
using ControleFinanceiro.Domain.Exceptions;
using ControleFinanceiro.Domain.ValueObjects;
using FluentAssertions;

namespace ControleFinanceiro.Domain.Tests.Entities;

public class TransacaoTests
{
    #region Properties
    #endregion

    #region Constructor
    #endregion

    [Fact]
    public void Deve_Criar_Transacao_Com_Dados_Validos()
    {
        var pessoa = CriarPessoaMaiorDeIdade();
        var categoria = CriarCategoriaDespesa();
        var valor = new Dinheiro(100m);
        var data = DateTime.UtcNow.AddDays(-1);

        var transacao = new Transacao(pessoa, categoria, TipoTransacao.Despesa, valor, data);

        transacao.Pessoa.Should().Be(pessoa);
        transacao.Categoria.Should().Be(categoria);
        transacao.Tipo.Should().Be(TipoTransacao.Despesa);
        transacao.Valor.Should().Be(valor);
        transacao.Data.Should().Be(data);
    }

    [Fact]
    public void Deve_Criar_Transacao_Com_Descricao()
    {
        var pessoa = CriarPessoaMaiorDeIdade();
        var categoria = CriarCategoriaDespesa();
        var valor = new Dinheiro(100m);
        var data = DateTime.UtcNow.AddDays(-1);
        var descricao = new Descricao("Descrição da transação");

        var transacao = new Transacao(pessoa, categoria, TipoTransacao.Despesa, valor, data, descricao);

        transacao.Descricao.Should().Be(descricao);
    }

    [Fact]
    public void Deve_Lancar_Excecao_Quando_Data_No_Futuro()
    {
        var pessoa = CriarPessoaMaiorDeIdade();
        var categoria = CriarCategoriaDespesa();
        var valor = new Dinheiro(100m);
        var dataFutura = DateTime.UtcNow.AddDays(1);

        Action act = () => new Transacao(pessoa, categoria, TipoTransacao.Despesa, valor, dataFutura);

        act.Should().Throw<DomainException>()
            .WithMessage("A data da transação não pode ser no futuro.");
    }

    [Fact]
    public void Deve_Lancar_Excecao_Quando_Menor_Tentar_Criar_Receita()
    {
        var pessoa = CriarPessoaMenorDeIdade();
        var categoria = CriarCategoriaReceita();
        var valor = new Dinheiro(100m);
        var data = DateTime.UtcNow.AddDays(-1);

        Action act = () => new Transacao(pessoa, categoria, TipoTransacao.Receita, valor, data);

        act.Should().Throw<DomainException>()
            .WithMessage("Menores de 18 anos só podem ter despesas.");
    }

    [Fact]
    public void Deve_Lancar_Excecao_Quando_Categoria_Incompativel()
    {
        var pessoa = CriarPessoaMaiorDeIdade();
        var categoria = CriarCategoriaDespesa();
        var valor = new Dinheiro(100m);
        var data = DateTime.UtcNow.AddDays(-1);

        Action act = () => new Transacao(pessoa, categoria, TipoTransacao.Receita, valor, data);

        act.Should().Throw<DomainException>()
            .Which.Message.Should().Contain("é exclusiva para despesas e não pode ser usada para receita");
    }

    [Fact]
    public void Deve_Atualizar_Valor()
    {
        var pessoa = CriarPessoaMaiorDeIdade();
        var categoria = CriarCategoriaDespesa();
        var valor = new Dinheiro(100m);
        var data = DateTime.UtcNow.AddDays(-1);
        var transacao = new Transacao(pessoa, categoria, TipoTransacao.Despesa, valor, data);

        var novoValor = new Dinheiro(200m);
        transacao.AtualizarValor(novoValor);

        transacao.Valor.Should().Be(novoValor);
    }

    [Fact]
    public void Deve_Atualizar_Data()
    {
        var pessoa = CriarPessoaMaiorDeIdade();
        var categoria = CriarCategoriaDespesa();
        var valor = new Dinheiro(100m);
        var data = DateTime.UtcNow.AddDays(-1);
        var transacao = new Transacao(pessoa, categoria, TipoTransacao.Despesa, valor, data);

        var novaData = DateTime.UtcNow.AddDays(-2);
        transacao.AtualizarData(novaData);

        transacao.Data.Should().Be(novaData);
    }

    [Fact]
    public void Deve_Lancar_Excecao_Ao_Atualizar_Data_No_Futuro()
    {
        var pessoa = CriarPessoaMaiorDeIdade();
        var categoria = CriarCategoriaDespesa();
        var valor = new Dinheiro(100m);
        var data = DateTime.UtcNow.AddDays(-1);
        var transacao = new Transacao(pessoa, categoria, TipoTransacao.Despesa, valor, data);

        var dataFutura = DateTime.UtcNow.AddDays(1);

        Action act = () => transacao.AtualizarData(dataFutura);

        act.Should().Throw<DomainException>()
            .WithMessage("A data da transação não pode ser no futuro.");
    }

    [Fact]
    public void Deve_Atualizar_Descricao()
    {
        var pessoa = CriarPessoaMaiorDeIdade();
        var categoria = CriarCategoriaDespesa();
        var valor = new Dinheiro(100m);
        var data = DateTime.UtcNow.AddDays(-1);
        var transacao = new Transacao(pessoa, categoria, TipoTransacao.Despesa, valor, data);

        var novaDescricao = new Descricao("Nova descrição");
        transacao.AtualizarDescricao(novaDescricao);

        transacao.Descricao.Should().Be(novaDescricao);
    }

    [Fact]
    public void Deve_Atualizar_Categoria()
    {
        var pessoa = CriarPessoaMaiorDeIdade();
        var categoria = CriarCategoriaDespesa();
        var valor = new Dinheiro(100m);
        var data = DateTime.UtcNow.AddDays(-1);
        var transacao = new Transacao(pessoa, categoria, TipoTransacao.Despesa, valor, data);

        var novaCategoria = CriarCategoriaAmbas();
        transacao.AtualizarCategoria(novaCategoria);

        transacao.Categoria.Should().Be(novaCategoria);
        transacao.CategoriaId.Should().Be(novaCategoria.Id);
    }

    [Fact]
    public void Deve_Lancar_Excecao_Ao_Atualizar_Categoria_Incompativel()
    {
        var pessoa = CriarPessoaMaiorDeIdade();
        var categoria = CriarCategoriaDespesa();
        var valor = new Dinheiro(100m);
        var data = DateTime.UtcNow.AddDays(-1);
        var transacao = new Transacao(pessoa, categoria, TipoTransacao.Despesa, valor, data);

        var categoriaReceita = CriarCategoriaReceita();

        Action act = () => transacao.AtualizarCategoria(categoriaReceita);

        act.Should().Throw<DomainException>()
            .Which.Message.Should().Contain("é exclusiva para receitas e não pode ser usada para despesa");
    }

    private Pessoa CriarPessoaMaiorDeIdade()
    {
        var nome = new Nome("João Silva");
        var cpf = new Cpf("11144477735");
        var dataNascimento = DateTime.UtcNow.AddYears(-25);
        return new Pessoa(nome, cpf, dataNascimento);
    }

    private Pessoa CriarPessoaMenorDeIdade()
    {
        var nome = new Nome("João Silva");
        var cpf = new Cpf("11144477735");
        var dataNascimento = DateTime.UtcNow.AddYears(-17);
        return new Pessoa(nome, cpf, dataNascimento);
    }

    private Categoria CriarCategoriaDespesa()
    {
        var nome = new Nome("Alimentação");
        var descricao = new Descricao("Gastos com alimentação");
        return new Categoria(nome, descricao, FinalidadeCategoria.Despesa);
    }

    private Categoria CriarCategoriaReceita()
    {
        var nome = new Nome("Salário");
        var descricao = new Descricao("Recebimento de salário");
        return new Categoria(nome, descricao, FinalidadeCategoria.Receita);
    }

    private Categoria CriarCategoriaAmbas()
    {
        var nome = new Nome("Geral");
        var descricao = new Descricao("Categoria geral");
        return new Categoria(nome, descricao, FinalidadeCategoria.Ambas);
    }
}
