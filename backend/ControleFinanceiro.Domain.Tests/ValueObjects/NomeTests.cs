using ControleFinanceiro.Domain.Exceptions;
using ControleFinanceiro.Domain.ValueObjects;
using FluentAssertions;

namespace ControleFinanceiro.Domain.Tests.ValueObjects;

public class NomeTests
{
    #region Properties
    #endregion

    #region Constructor
    #endregion

    [Fact]
    public void Deve_Criar_Nome_Valido()
    {
        var nomeValido = "João Silva";

        var nome = new Nome(nomeValido);

        nome.Valor.Should().Be(nomeValido);
    }

    [Fact]
    public void Deve_Trim_No_Nome()
    {
        var nomeComEspacos = "  João Silva  ";

        var nome = new Nome(nomeComEspacos);

        nome.Valor.Should().Be("João Silva");
    }

    [Fact]
    public void Deve_Lancar_Excecao_Quando_Nome_Vazio()
    {
        Action act = () => new Nome(string.Empty);

        act.Should().Throw<DomainException>()
            .WithMessage("O nome não pode ser vazio.");
    }

    [Fact]
    public void Deve_Lancar_Excecao_Quando_Nome_Null()
    {
        Action act = () => new Nome(null!);

        act.Should().Throw<DomainException>()
            .WithMessage("O nome não pode ser vazio.");
    }

    [Fact]
    public void Deve_Lancar_Excecao_Quando_Nome_Apenas_Espacos()
    {
        Action act = () => new Nome("   ");

        act.Should().Throw<DomainException>()
            .WithMessage("O nome não pode ser vazio.");
    }

    [Fact]
    public void Deve_Lancar_Excecao_Quando_Nome_Muito_Longo()
    {
        var nomeMuitoLongo = new string('A', 101);

        Action act = () => new Nome(nomeMuitoLongo);

        act.Should().Throw<DomainException>()
            .WithMessage("O nome não pode ter mais de 100 caracteres.");
    }

    [Fact]
    public void Deve_Aceitar_Nome_Com_100_Caracteres()
    {
        var nomeCom100Caracteres = new string('A', 100);

        var nome = new Nome(nomeCom100Caracteres);

        nome.Valor.Should().Be(nomeCom100Caracteres);
    }

    [Fact]
    public void Deve_Retornar_True_Quando_Nomes_Sao_Iguais()
    {
        var nome1 = new Nome("João Silva");
        var nome2 = new Nome("João Silva");

        var saoIguais = nome1.Equals(nome2);

        saoIguais.Should().BeTrue();
    }

    [Fact]
    public void Deve_Retornar_False_Quando_Nomes_Sao_Diferentes()
    {
        var nome1 = new Nome("João Silva");
        var nome2 = new Nome("Maria Santos");

        var saoIguais = nome1.Equals(nome2);

        saoIguais.Should().BeFalse();
    }

    [Fact]
    public void Deve_Retornar_Mesmo_HashCode_Para_Nomes_Iguais()
    {
        var nome1 = new Nome("João Silva");
        var nome2 = new Nome("João Silva");

        var hashCode1 = nome1.GetHashCode();
        var hashCode2 = nome2.GetHashCode();

        hashCode1.Should().Be(hashCode2);
    }

    [Fact]
    public void Deve_Converter_Implicitamente_Para_String()
    {
        var nome = new Nome("João Silva");

        string nomeString = nome;

        nomeString.Should().Be("João Silva");
    }

    [Fact]
    public void Deve_Retornar_Valor_No_ToString()
    {
        var nome = new Nome("João Silva");

        var resultado = nome.ToString();

        resultado.Should().Be("João Silva");
    }
}
