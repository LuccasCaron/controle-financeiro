using ControleFinanceiro.Application.Common;
using ControleFinanceiro.Application.Transacoes.Queries;
using ControleFinanceiro.Application.Transacoes.Queries.Views;
using ControleFinanceiro.Domain.Repositories;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace ControleFinanceiro.Application.Transacoes.Queries.Handlers;

public class ListarTransacoesQueryHandler : IRequestHandler<ListarTransacoesQuery, ListarTransacoesView>
{
    #region Properties

    private readonly ITransacaoRepository _transacaoRepository;

    #endregion

    #region Constructor

    public ListarTransacoesQueryHandler(ITransacaoRepository transacaoRepository)
    {
        _transacaoRepository = transacaoRepository;
    }

    #endregion

    public async Task<ListarTransacoesView> Handle(ListarTransacoesQuery request, CancellationToken cancellationToken)
    {
        var query = _transacaoRepository.ObterQueryable();

        var totalCount = await query.CountAsync(cancellationToken);

        var transacoes = await query
            .Select(t => new TransacaoView
            {
                Id = t.Id,
                PessoaId = t.PessoaId,
                PessoaNome = t.Pessoa.Nome.Valor,
                CategoriaId = t.CategoriaId,
                CategoriaNome = t.Categoria.Nome.Valor,
                Tipo = t.Tipo,
                Valor = t.Valor.Valor,
                Data = t.Data,
                Descricao = t.Descricao != null ? t.Descricao.Valor : null
            })
            .Skip((request.Page - 1) * request.PageSize)
            .Take(request.PageSize)
            .ToListAsync(cancellationToken);

        var pagedResponse = new PagedResponse<TransacaoView>(transacoes, totalCount, request.Page, request.PageSize);

        return new ListarTransacoesView(pagedResponse);
    }
}
