using ControleFinanceiro.Domain.Exceptions;
using ControleFinanceiro.Domain.ValueObjects;
using FluentAssertions;

namespace ControleFinanceiro.Domain.Tests.ValueObjects;

public class DinheiroTests
{
    #region Properties
    #endregion

    #region Constructor
    #endregion

    [Fact]
    public void Deve_Criar_Dinheiro_Valido()
    {
        var valor = 100.50m;

        var dinheiro = new Dinheiro(valor);

        dinheiro.Valor.Should().Be(valor);
    }

    [Fact]
    public void Deve_Arredondar_Valor_Para_Duas_Casas_Decimais()
    {
        var valor = 100.555m;
        var valorEsperado = 100.56m;

        var dinheiro = new Dinheiro(valor);

        dinheiro.Valor.Should().Be(valorEsperado);
    }

    [Fact]
    public void Deve_Lancar_Excecao_Quando_Valor_Zero()
    {
        Action act = () => new Dinheiro(0m);

        act.Should().Throw<DomainException>()
            .WithMessage("O valor monetário deve ser maior que zero.");
    }

    [Fact]
    public void Deve_Lancar_Excecao_Quando_Valor_Negativo()
    {
        Action act = () => new Dinheiro(-10m);

        act.Should().Throw<DomainException>()
            .WithMessage("O valor monetário deve ser maior que zero.");
    }

    [Fact]
    public void Deve_Somar_Dois_Valores_Dinheiro()
    {
        var dinheiro1 = new Dinheiro(100m);
        var dinheiro2 = new Dinheiro(50m);

        var resultado = dinheiro1 + dinheiro2;

        resultado.Valor.Should().Be(150m);
    }

    [Fact]
    public void Deve_Subtrair_Dois_Valores_Dinheiro()
    {
        var dinheiro1 = new Dinheiro(100m);
        var dinheiro2 = new Dinheiro(30m);

        var resultado = dinheiro1 - dinheiro2;

        resultado.Valor.Should().Be(70m);
    }

    [Fact]
    public void Deve_Multiplicar_Dinheiro_Por_Decimal()
    {
        var dinheiro = new Dinheiro(100m);
        var multiplicador = 1.5m;

        var resultado = dinheiro * multiplicador;

        resultado.Valor.Should().Be(150m);
    }

    [Fact]
    public void Deve_Retornar_True_Quando_Valores_Sao_Iguais()
    {
        var dinheiro1 = new Dinheiro(100m);
        var dinheiro2 = new Dinheiro(100m);

        var saoIguais = dinheiro1.Equals(dinheiro2);

        saoIguais.Should().BeTrue();
    }

    [Fact]
    public void Deve_Retornar_False_Quando_Valores_Sao_Diferentes()
    {
        var dinheiro1 = new Dinheiro(100m);
        var dinheiro2 = new Dinheiro(200m);

        var saoIguais = dinheiro1.Equals(dinheiro2);

        saoIguais.Should().BeFalse();
    }

    [Fact]
    public void Deve_Retornar_Mesmo_HashCode_Para_Valores_Iguais()
    {
        var dinheiro1 = new Dinheiro(100m);
        var dinheiro2 = new Dinheiro(100m);

        var hashCode1 = dinheiro1.GetHashCode();
        var hashCode2 = dinheiro2.GetHashCode();

        hashCode1.Should().Be(hashCode2);
    }

    [Fact]
    public void Deve_Converter_Implicitamente_Para_Decimal()
    {
        var dinheiro = new Dinheiro(100.50m);

        decimal valorDecimal = dinheiro;

        valorDecimal.Should().Be(100.50m);
    }

    [Fact]
    public void Deve_Retornar_Valor_Formatado_No_ToString()
    {
        var dinheiro = new Dinheiro(100.50m);

        var resultado = dinheiro.ToString();

        resultado.Should().Be("100,50");
    }
}
