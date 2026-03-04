using ControleFinanceiro.Application.Common;
using ControleFinanceiro.Application.Pessoas.Queries;
using ControleFinanceiro.Application.Pessoas.Queries.Views;
using ControleFinanceiro.Domain.Repositories;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace ControleFinanceiro.Application.Pessoas.Handlers;

public class ListarPessoasQueryHandler : IRequestHandler<ListarPessoasQuery, ListarPessoasView>
{
    #region Properties

    private readonly IPessoaRepository _pessoaRepository;

    #endregion

    #region Constructor

    public ListarPessoasQueryHandler(IPessoaRepository pessoaRepository)
    {
        _pessoaRepository = pessoaRepository;
    }

    #endregion

    public async Task<ListarPessoasView> Handle(ListarPessoasQuery request, CancellationToken cancellationToken)
    {
        var query = _pessoaRepository.ObterQueryable();

        var totalCount = await query.CountAsync(cancellationToken);

        var pessoas = await query
            .Select(p => new PessoaView
            {
                Id = p.Id,
                Nome = p.Nome.Valor,
                Cpf = p.Cpf.Valor,
                DataNascimento = p.DataNascimento,
                Idade = p.Idade
            })
            .Skip((request.Page - 1) * request.PageSize)
            .Take(request.PageSize)
            .ToListAsync(cancellationToken);

        var pagedResponse = new PagedResponse<PessoaView>(pessoas, totalCount, request.Page, request.PageSize);

        return new ListarPessoasView(pagedResponse);
    }
}
