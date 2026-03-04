using ControleFinanceiro.Application.Categorias.Queries.Views;
using MediatR;

namespace ControleFinanceiro.Application.Categorias.Queries;

public class ListarCategoriasQuery : IRequest<ListarCategoriasView>
{
    public int Page { get; private set; }
    public int PageSize { get; private set; }

    public ListarCategoriasQuery(int page = 1, int pageSize = 10)
    {
        Page = page < 1 ? 1 : page;
        PageSize = pageSize < 1 ? 10 : (pageSize > 100 ? 100 : pageSize);
    }
}
