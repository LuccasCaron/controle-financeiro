using ControleFinanceiro.Application.Pessoas.Queries.Views;
using MediatR;

namespace ControleFinanceiro.Application.Pessoas.Queries;

public class ObterPessoaPorIdQuery : IRequest<ObterPessoaPorIdView>
{
    public Guid Id { get; private set; }

    public ObterPessoaPorIdQuery(Guid id)
    {
        Id = id;
    }
}
