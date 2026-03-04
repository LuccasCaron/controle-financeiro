using ControleFinanceiro.Domain.Exceptions;

namespace ControleFinanceiro.Domain.ValueObjects;

public class Nome
{
    public string Valor { get; private set; }

    private const int TamanhoMaximo = 100;

    private Nome()
    {
    }

    public Nome(string valor)
    {
        if (string.IsNullOrWhiteSpace(valor))
        {
            throw new DomainException("O nome não pode ser vazio.");
        }

        if (valor.Length > TamanhoMaximo)
        {
            throw new DomainException($"O nome não pode ter mais de {TamanhoMaximo} caracteres.");
        }

        Valor = valor.Trim();
    }

    public override string ToString() => Valor;

    public override bool Equals(object? obj)
    {
        if (obj is not Nome other)
            return false;

        return Valor == other.Valor;
    }

    public override int GetHashCode() => Valor.GetHashCode();

    public static implicit operator string(Nome nome) => nome.Valor;
}
