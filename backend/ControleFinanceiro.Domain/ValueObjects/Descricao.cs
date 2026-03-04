using ControleFinanceiro.Domain.Exceptions;

namespace ControleFinanceiro.Domain.ValueObjects;

public class Descricao
{
    public string? Valor { get; private set; }

    private const int TamanhoMaximo = 500;

    public bool EstaVazia => string.IsNullOrWhiteSpace(Valor);

    private Descricao()
    {
    }

    public Descricao(string? valor)
    {
        if (!string.IsNullOrWhiteSpace(valor) && valor.Length > TamanhoMaximo)
        {
            throw new DomainException($"A descrição não pode ter mais de {TamanhoMaximo} caracteres.");
        }

        Valor = string.IsNullOrWhiteSpace(valor) ? null : valor.Trim();
    }

    public override string ToString() => Valor ?? string.Empty;

    public override bool Equals(object? obj)
    {
        if (obj is not Descricao other)
            return false;

        return Valor == other.Valor;
    }

    public override int GetHashCode() => Valor?.GetHashCode() ?? 0;

    public static implicit operator string?(Descricao descricao) => descricao.Valor;
}
