using ControleFinanceiro.Domain.Exceptions;
using ControleFinanceiro.Domain.ValueObjects;
using FluentAssertions;

namespace ControleFinanceiro.Domain.Tests.ValueObjects;

public class DescricaoTests
{
    #region Properties
    #endregion

    #region Constructor
    #endregion

    [Fact]
    public void Deve_Criar_Descricao_Valida()
    {
        var descricaoValida = "Descrição válida";

        var descricao = new Descricao(descricaoValida);

        descricao.Valor.Should().Be(descricaoValida);
    }

    [Fact]
    public void Deve_Aceitar_Descricao_Null()
    {
        var descricao = new Descricao(null);

        descricao.Valor.Should().BeNull();
        descricao.EstaVazia.Should().BeTrue();
    }

    [Fact]
    public void Deve_Aceitar_Descricao_Vazia()
    {
        var descricao = new Descricao(string.Empty);

        descricao.Valor.Should().BeNull();
        descricao.EstaVazia.Should().BeTrue();
    }

    [Fact]
    public void Deve_Aceitar_Descricao_Apenas_Espacos()
    {
        var descricao = new Descricao("   ");

        descricao.Valor.Should().BeNull();
        descricao.EstaVazia.Should().BeTrue();
    }

    [Fact]
    public void Deve_Trim_Na_Descricao()
    {
        var descricaoComEspacos = "  Descrição com espaços  ";

        var descricao = new Descricao(descricaoComEspacos);

        descricao.Valor.Should().Be("Descrição com espaços");
    }

    [Fact]
    public void Deve_Lancar_Excecao_Quando_Descricao_Muito_Longa()
    {
        var descricaoMuitoLonga = new string('A', 501);

        Action act = () => new Descricao(descricaoMuitoLonga);

        act.Should().Throw<DomainException>()
            .WithMessage("A descrição não pode ter mais de 500 caracteres.");
    }

    [Fact]
    public void Deve_Aceitar_Descricao_Com_500_Caracteres()
    {
        var descricaoCom500Caracteres = new string('A', 500);

        var descricao = new Descricao(descricaoCom500Caracteres);

        descricao.Valor.Should().Be(descricaoCom500Caracteres);
    }

    [Fact]
    public void Deve_Retornar_True_Quando_Descricoes_Sao_Iguais()
    {
        var descricao1 = new Descricao("Descrição");
        var descricao2 = new Descricao("Descrição");

        var saoIguais = descricao1.Equals(descricao2);

        saoIguais.Should().BeTrue();
    }

    [Fact]
    public void Deve_Retornar_True_Quando_Ambas_Descricoes_Sao_Null()
    {
        var descricao1 = new Descricao(null);
        var descricao2 = new Descricao(null);

        var saoIguais = descricao1.Equals(descricao2);

        saoIguais.Should().BeTrue();
    }

    [Fact]
    public void Deve_Retornar_False_Quando_Descricoes_Sao_Diferentes()
    {
        var descricao1 = new Descricao("Descrição 1");
        var descricao2 = new Descricao("Descrição 2");

        var saoIguais = descricao1.Equals(descricao2);

        saoIguais.Should().BeFalse();
    }

    [Fact]
    public void Deve_Retornar_Mesmo_HashCode_Para_Descricoes_Iguais()
    {
        var descricao1 = new Descricao("Descrição");
        var descricao2 = new Descricao("Descrição");

        var hashCode1 = descricao1.GetHashCode();
        var hashCode2 = descricao2.GetHashCode();

        hashCode1.Should().Be(hashCode2);
    }

    [Fact]
    public void Deve_Converter_Implicitamente_Para_String()
    {
        var descricao = new Descricao("Descrição");

        string? descricaoString = descricao;

        descricaoString.Should().Be("Descrição");
    }

    [Fact]
    public void Deve_Retornar_String_Vazia_No_ToString_Quando_Null()
    {
        var descricao = new Descricao(null);

        var resultado = descricao.ToString();

        resultado.Should().Be(string.Empty);
    }

    [Fact]
    public void Deve_Retornar_Valor_No_ToString_Quando_Valor_Existe()
    {
        var descricao = new Descricao("Descrição");

        var resultado = descricao.ToString();

        resultado.Should().Be("Descrição");
    }
}
