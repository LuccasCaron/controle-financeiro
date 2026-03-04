using ControleFinanceiro.Domain.Exceptions;
using ControleFinanceiro.Domain.ValueObjects;
using FluentAssertions;

namespace ControleFinanceiro.Domain.Tests.ValueObjects;

public class CpfTests
{
    #region Properties
    #endregion

    #region Constructor
    #endregion

    [Fact]
    public void Deve_Criar_Cpf_Valido()
    {
        var cpfValido = "11144477735";

        var cpf = new Cpf(cpfValido);

        cpf.Valor.Should().Be(cpfValido);
    }

    [Fact]
    public void Deve_Criar_Cpf_Com_Formatacao()
    {
        var cpfComFormatacao = "111.444.777-35";
        var cpfEsperado = "11144477735";

        var cpf = new Cpf(cpfComFormatacao);

        cpf.Valor.Should().Be(cpfEsperado);
    }

    [Fact]
    public void Deve_Lancar_Excecao_Quando_Cpf_Vazio()
    {
        Action act = static () => new Cpf(string.Empty);

        act.Should().Throw<DomainException>()
            .WithMessage("O CPF não pode ser vazio.");
    }

    [Fact]
    public void Deve_Lancar_Excecao_Quando_Cpf_Null()
    {
        Action act = () => new Cpf(null!);

        act.Should().Throw<DomainException>()
            .WithMessage("O CPF não pode ser vazio.");
    }

    [Fact]
    public void Deve_Lancar_Excecao_Quando_Cpf_Com_Formato_Incorreto()
    {
        Action act = () => new Cpf("123456789");

        act.Should().Throw<DomainException>()
            .WithMessage("O CPF deve conter exatamente 11 dígitos numéricos.");
    }

    [Fact]
    public void Deve_Lancar_Excecao_Quando_Cpf_Com_Digitos_Verificadores_Incorretos()
    {
        Action act = () => new Cpf("11144477730");

        act.Should().Throw<DomainException>()
            .WithMessage("CPF inválido.");
    }

    [Fact]
    public void Deve_Lancar_Excecao_Quando_Todos_Digitos_Iguais()
    {
        Action act = () => new Cpf("11111111111");

        act.Should().Throw<DomainException>()
            .WithMessage("CPF inválido.");
    }

    [Fact]
    public void Deve_Retornar_True_Quando_Cpfs_Sao_Iguais()
    {
        var cpf1 = new Cpf("11144477735");
        var cpf2 = new Cpf("11144477735");

        var saoIguais = cpf1.Equals(cpf2);

        saoIguais.Should().BeTrue();
    }

    [Fact]
    public void Deve_Retornar_False_Quando_Cpfs_Sao_Diferentes()
    {
        var cpf1 = new Cpf("11144477735");
        var cpf2 = new Cpf("12345678909");

        var saoIguais = cpf1.Equals(cpf2);

        saoIguais.Should().BeFalse();
    }

    [Fact]
    public void Deve_Retornar_Mesmo_HashCode_Para_Cpfs_Iguais()
    {
        var cpf1 = new Cpf("11144477735");
        var cpf2 = new Cpf("11144477735");

        var hashCode1 = cpf1.GetHashCode();
        var hashCode2 = cpf2.GetHashCode();

        hashCode1.Should().Be(hashCode2);
    }

    [Fact]
    public void Deve_Converter_Implicitamente_Para_String()
    {
        var cpf = new Cpf("11144477735");

        string cpfString = cpf;

        cpfString.Should().Be("11144477735");
    }

    [Fact]
    public void Deve_Retornar_Valor_No_ToString()
    {
        var cpf = new Cpf("11144477735");

        var resultado = cpf.ToString();

        resultado.Should().Be("11144477735");
    }
}
