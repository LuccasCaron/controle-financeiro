using ControleFinanceiro.Application.Common;
using ControleFinanceiro.Application.Categorias.Queries;
using ControleFinanceiro.Application.Categorias.Queries.Views;
using ControleFinanceiro.Domain.Repositories;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace ControleFinanceiro.Application.Categorias.Queries.Handlers;

public class ListarCategoriasQueryHandler : IRequestHandler<ListarCategoriasQuery, ListarCategoriasView>
{
    #region Properties

    private readonly ICategoriaRepository _categoriaRepository;

    #endregion

    #region Constructor

    public ListarCategoriasQueryHandler(ICategoriaRepository categoriaRepository)
    {
        _categoriaRepository = categoriaRepository;
    }

    #endregion

    public async Task<ListarCategoriasView> Handle(ListarCategoriasQuery request, CancellationToken cancellationToken)
    {
        var query = _categoriaRepository.ObterQueryable();

        var totalCount = await query.CountAsync(cancellationToken);

        var categorias = await query
            .Select(c => new CategoriaView
            {
                Id = c.Id,
                Nome = c.Nome.Valor,
                Descricao = c.Descricao.Valor,
                Finalidade = c.Finalidade
            })
            .Skip((request.Page - 1) * request.PageSize)
            .Take(request.PageSize)
            .ToListAsync(cancellationToken);

        var pagedResponse = new PagedResponse<CategoriaView>(categorias, totalCount, request.Page, request.PageSize);

        return new ListarCategoriasView(pagedResponse);
    }
}
