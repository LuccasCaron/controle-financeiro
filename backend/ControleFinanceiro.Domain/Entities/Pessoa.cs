using ControleFinanceiro.Domain.Common;
using ControleFinanceiro.Domain.Exceptions;
using ControleFinanceiro.Domain.ValueObjects;

namespace ControleFinanceiro.Domain.Entities;

public class Pessoa : BaseEntity
{
    public Nome Nome { get; private set; } = null!;
    public Cpf Cpf { get; private set; } = null!;
    public DateTime DataNascimento { get; private set; }

    public int Idade
    {
        get
        {
            var hoje = DateTime.UtcNow;
            var idade = hoje.Year - DataNascimento.Year;

            if (hoje.Month < DataNascimento.Month ||
                (hoje.Month == DataNascimento.Month && hoje.Day < DataNascimento.Day))
            {
                idade--;
            }

            return idade;
        }
    }

    public bool EMenorDeIdade => Idade < 18;

    private Pessoa()
    {
    }

    public Pessoa(Nome nome, Cpf cpf, DateTime dataNascimento)
    {
        if (dataNascimento > DateTime.UtcNow)
        {
            throw new DomainException("A data de nascimento não pode ser no futuro.");
        }

        var idadeMaxima = 150;
        var dataMinima = DateTime.UtcNow.AddYears(-idadeMaxima);
        if (dataNascimento < dataMinima)
        {
            throw new DomainException($"A data de nascimento não pode ser anterior a {dataMinima:dd/MM/yyyy}.");
        }

        Nome = nome;
        Cpf = cpf;
        DataNascimento = dataNascimento;
    }

    public void AtualizarNome(Nome novoNome)
    {
        Nome = novoNome;
    }

    public void AtualizarDataNascimento(DateTime novaDataNascimento)
    {
        if (novaDataNascimento > DateTime.UtcNow)
        {
            throw new DomainException("A data de nascimento não pode ser no futuro.");
        }

        var idadeMaxima = 150;
        var dataMinima = DateTime.UtcNow.AddYears(-idadeMaxima);
        if (novaDataNascimento < dataMinima)
        {
            throw new DomainException($"A data de nascimento não pode ser anterior a {dataMinima:dd/MM/yyyy}.");
        }

        DataNascimento = novaDataNascimento;
    }

    public void ValidarTipoTransacaoPermitido(Enums.TipoTransacao tipoTransacao)
    {
        if (EMenorDeIdade && tipoTransacao == Enums.TipoTransacao.Receita)
        {
            throw new DomainException("Menores de 18 anos só podem ter despesas.");
        }
    }
}
