using ControleFinanceiro.Application.Common;
using MediatR;

namespace ControleFinanceiro.Application.Pessoas.Commands;

public class AtualizarPessoaCommand : IRequest<BaseView>
{
    public Guid Id { get; private set; }
    public string Nome { get; private set; }
    public DateTime DataNascimento { get; private set; }

    public AtualizarPessoaCommand(Guid id, string nome, DateTime dataNascimento)
    {
        Id = id;
        Nome = nome;
        DataNascimento = dataNascimento;
    }
}
