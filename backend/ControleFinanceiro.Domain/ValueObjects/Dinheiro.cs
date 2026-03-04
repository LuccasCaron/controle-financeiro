using ControleFinanceiro.Domain.Exceptions;

namespace ControleFinanceiro.Domain.ValueObjects;

public class Dinheiro
{
    public decimal Valor { get; private set; }

    private Dinheiro()
    {
    }

    public Dinheiro(decimal valor)
    {
        if (valor <= 0)
        {
            throw new DomainException("O valor monetário deve ser maior que zero.");
        }

        Valor = Math.Round(valor, 2, MidpointRounding.AwayFromZero);
    }

    public static Dinheiro operator +(Dinheiro a, Dinheiro b)
    {
        return new Dinheiro(a.Valor + b.Valor);
    }

    public static Dinheiro operator -(Dinheiro a, Dinheiro b)
    {
        return new Dinheiro(a.Valor - b.Valor);
    }

    public static Dinheiro operator *(Dinheiro a, decimal multiplicador)
    {
        return new Dinheiro(a.Valor * multiplicador);
    }

    public override string ToString() => Valor.ToString("F2");

    public override bool Equals(object? obj)
    {
        if (obj is not Dinheiro other)
            return false;

        return Valor == other.Valor;
    }

    public override int GetHashCode() => Valor.GetHashCode();

    public static implicit operator decimal(Dinheiro dinheiro) => dinheiro.Valor;
}
