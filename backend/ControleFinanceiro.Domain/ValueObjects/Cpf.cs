using System.Linq;
using System.Text.RegularExpressions;
using ControleFinanceiro.Domain.Exceptions;

namespace ControleFinanceiro.Domain.ValueObjects;

public class Cpf
{
    public string Valor { get; private set; }

    private static readonly Regex CpfRegex = new(@"^\d{11}$", RegexOptions.Compiled);

    private Cpf()
    {
    }

    public Cpf(string valor)
    {
        if (string.IsNullOrWhiteSpace(valor))
        {
            throw new DomainException("O CPF não pode ser vazio.");
        }

        var cpfLimpo = RemoverFormatacao(valor);

        if (!CpfRegex.IsMatch(cpfLimpo))
        {
            throw new DomainException("O CPF deve conter exatamente 11 dígitos numéricos.");
        }

        if (!ValidarDigitosVerificadores(cpfLimpo))
        {
            throw new DomainException("CPF inválido.");
        }

        Valor = cpfLimpo;
    }

    private static string RemoverFormatacao(string cpf)
    {
        return Regex.Replace(cpf, @"[^\d]", string.Empty);
    }

    private static bool ValidarDigitosVerificadores(string cpf)
    {
        if (cpf.Length != 11)
            return false;

        if (cpf.Distinct().Count() == 1)
            return false;

        var multiplicador1 = new[] { 10, 9, 8, 7, 6, 5, 4, 3, 2 };
        var multiplicador2 = new[] { 11, 10, 9, 8, 7, 6, 5, 4, 3, 2 };

        var tempCpf = cpf.Substring(0, 9);
        var soma = 0;

        for (var i = 0; i < 9; i++)
        {
            soma += int.Parse(tempCpf[i].ToString()) * multiplicador1[i];
        }

        var resto = soma % 11;
        var digito1 = resto < 2 ? 0 : 11 - resto;

        tempCpf += digito1;
        soma = 0;

        for (var i = 0; i < 10; i++)
        {
            soma += int.Parse(tempCpf[i].ToString()) * multiplicador2[i];
        }

        resto = soma % 11;
        var digito2 = resto < 2 ? 0 : 11 - resto;

        return cpf.EndsWith(digito1.ToString() + digito2.ToString());
    }

    public override string ToString() => Valor;

    public override bool Equals(object? obj)
    {
        if (obj is not Cpf other)
            return false;

        return Valor == other.Valor;
    }

    public override int GetHashCode() => Valor.GetHashCode();

    public static implicit operator string(Cpf cpf) => cpf.Valor;
}
