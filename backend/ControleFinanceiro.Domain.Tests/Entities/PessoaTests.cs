using ControleFinanceiro.Domain.Entities;
using ControleFinanceiro.Domain.Enums;
using ControleFinanceiro.Domain.Exceptions;
using ControleFinanceiro.Domain.ValueObjects;
using FluentAssertions;

namespace ControleFinanceiro.Domain.Tests.Entities;

public class PessoaTests
{
    #region Properties
    #endregion

    #region Constructor
    #endregion

    [Fact]
    public void Deve_Criar_Pessoa_Com_Dados_Validos()
    {
        var nome = new Nome("João Silva");
        var cpf = new Cpf("11144477735");
        var dataNascimento = new DateTime(1990, 1, 15, 0, 0, 0, DateTimeKind.Utc);

        var pessoa = new Pessoa(nome, cpf, dataNascimento);

        pessoa.Nome.Should().Be(nome);
        pessoa.Cpf.Should().Be(cpf);
        pessoa.DataNascimento.Should().Be(dataNascimento);
    }

    [Fact]
    public void Deve_Lancar_Excecao_Quando_Data_Nascimento_No_Futuro()
    {
        var nome = new Nome("João Silva");
        var cpf = new Cpf("11144477735");
        var dataFutura = DateTime.UtcNow.AddDays(1);

        Action act = () => new Pessoa(nome, cpf, dataFutura);

        act.Should().Throw<DomainException>()
            .WithMessage("A data de nascimento não pode ser no futuro.");
    }

    [Fact]
    public void Deve_Lancar_Excecao_Quando_Data_Nascimento_Muito_Antiga()
    {
        var nome = new Nome("João Silva");
        var cpf = new Cpf("11144477735");
        var dataMuitoAntiga = DateTime.UtcNow.AddYears(-151);

        Action act = () => new Pessoa(nome, cpf, dataMuitoAntiga);

        act.Should().Throw<DomainException>()
            .Which.Message.Should().Contain("A data de nascimento não pode ser anterior a");
    }

    [Fact]
    public void Deve_Calcular_Idade_Corretamente()
    {
        var nome = new Nome("João Silva");
        var cpf = new Cpf("11144477735");
        var dataNascimento = DateTime.UtcNow.AddYears(-25);

        var pessoa = new Pessoa(nome, cpf, dataNascimento);

        pessoa.Idade.Should().Be(25);
    }

    [Fact]
    public void Deve_Retornar_True_Para_EMenorDeIdade_Quando_Idade_Menor_Que_18()
    {
        var nome = new Nome("João Silva");
        var cpf = new Cpf("11144477735");
        var dataNascimento = DateTime.UtcNow.AddYears(-17);

        var pessoa = new Pessoa(nome, cpf, dataNascimento);

        pessoa.EMenorDeIdade.Should().BeTrue();
    }

    [Fact]
    public void Deve_Retornar_False_Para_EMenorDeIdade_Quando_Idade_Maior_Que_18()
    {
        var nome = new Nome("João Silva");
        var cpf = new Cpf("11144477735");
        var dataNascimento = DateTime.UtcNow.AddYears(-25);

        var pessoa = new Pessoa(nome, cpf, dataNascimento);

        pessoa.EMenorDeIdade.Should().BeFalse();
    }

    [Fact]
    public void Deve_Atualizar_Nome()
    {
        var nome = new Nome("João Silva");
        var cpf = new Cpf("11144477735");
        var dataNascimento = DateTime.UtcNow.AddYears(-25);
        var pessoa = new Pessoa(nome, cpf, dataNascimento);

        var novoNome = new Nome("João Santos");
        pessoa.AtualizarNome(novoNome);

        pessoa.Nome.Should().Be(novoNome);
    }

    [Fact]
    public void Deve_Atualizar_Data_Nascimento()
    {
        var nome = new Nome("João Silva");
        var cpf = new Cpf("11144477735");
        var dataNascimento = DateTime.UtcNow.AddYears(-25);
        var pessoa = new Pessoa(nome, cpf, dataNascimento);

        var novaDataNascimento = DateTime.UtcNow.AddYears(-30);
        pessoa.AtualizarDataNascimento(novaDataNascimento);

        pessoa.DataNascimento.Should().Be(novaDataNascimento);
    }

    [Fact]
    public void Deve_Lancar_Excecao_Ao_Atualizar_Data_Nascimento_No_Futuro()
    {
        var nome = new Nome("João Silva");
        var cpf = new Cpf("11144477735");
        var dataNascimento = DateTime.UtcNow.AddYears(-25);
        var pessoa = new Pessoa(nome, cpf, dataNascimento);

        var dataFutura = DateTime.UtcNow.AddDays(1);

        Action act = () => pessoa.AtualizarDataNascimento(dataFutura);

        act.Should().Throw<DomainException>()
            .WithMessage("A data de nascimento não pode ser no futuro.");
    }

    [Fact]
    public void Deve_Lancar_Excecao_Quando_Menor_Tentar_Validar_Receita()
    {
        var nome = new Nome("João Silva");
        var cpf = new Cpf("11144477735");
        var dataNascimento = DateTime.UtcNow.AddYears(-17);
        var pessoa = new Pessoa(nome, cpf, dataNascimento);

        Action act = () => pessoa.ValidarTipoTransacaoPermitido(TipoTransacao.Receita);

        act.Should().Throw<DomainException>()
            .WithMessage("Menores de 18 anos só podem ter despesas.");
    }

    [Fact]
    public void Deve_Permitir_Despesa_Para_Menor_De_Idade()
    {
        var nome = new Nome("João Silva");
        var cpf = new Cpf("11144477735");
        var dataNascimento = DateTime.UtcNow.AddYears(-17);
        var pessoa = new Pessoa(nome, cpf, dataNascimento);

        Action act = () => pessoa.ValidarTipoTransacaoPermitido(TipoTransacao.Despesa);

        act.Should().NotThrow();
    }

    [Fact]
    public void Deve_Permitir_Receita_Para_Maior_De_Idade()
    {
        var nome = new Nome("João Silva");
        var cpf = new Cpf("11144477735");
        var dataNascimento = DateTime.UtcNow.AddYears(-25);
        var pessoa = new Pessoa(nome, cpf, dataNascimento);

        Action act = () => pessoa.ValidarTipoTransacaoPermitido(TipoTransacao.Receita);

        act.Should().NotThrow();
    }
}
