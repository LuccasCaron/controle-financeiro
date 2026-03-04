using ControleFinanceiro.Application.Common;
using MediatR;

namespace ControleFinanceiro.Application.Pessoas.Commands;

public class ExcluirPessoaCommand : IRequest<BaseView>
{
    public Guid Id { get; private set; }

    public ExcluirPessoaCommand(Guid id)
    {
        Id = id;
    }
}
