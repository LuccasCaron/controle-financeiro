using ControleFinanceiro.Application.Common;
using MediatR;

namespace ControleFinanceiro.Application.Pessoas.Commands;

public class CriarPessoaCommand : IRequest<BaseView>
{
    public string Nome { get; private set; }
    public string Cpf { get; private set; }
    public DateTime DataNascimento { get; private set; }

    public CriarPessoaCommand(string nome, string cpf, DateTime dataNascimento)
    {
        Nome = nome;
        Cpf = cpf;
        DataNascimento = dataNascimento;
    }
}
