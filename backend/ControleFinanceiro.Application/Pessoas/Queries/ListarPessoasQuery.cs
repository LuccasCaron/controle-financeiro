using ControleFinanceiro.Application.Common;
using ControleFinanceiro.Application.Pessoas.Queries.Views;
using MediatR;

namespace ControleFinanceiro.Application.Pessoas.Queries;

public class ListarPessoasQuery : IRequest<ListarPessoasView>
{
    public int Page { get; private set; }
    public int PageSize { get; private set; }

    public ListarPessoasQuery(int page = 1, int pageSize = 10)
    {
        Page = page < 1 ? 1 : page;
        PageSize = pageSize < 1 ? 10 : (pageSize > 100 ? 100 : pageSize);
    }
}
