using ControleFinanceiro.Application.Pessoas.Queries;
using ControleFinanceiro.Application.Pessoas.Queries.Views;
using ControleFinanceiro.Domain.Repositories;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace ControleFinanceiro.Application.Pessoas.Handlers;

public class ObterPessoaPorIdQueryHandler : IRequestHandler<ObterPessoaPorIdQuery, ObterPessoaPorIdView>
{
    #region Properties

    private readonly IPessoaRepository _pessoaRepository;

    #endregion

    #region Constructor

    public ObterPessoaPorIdQueryHandler(IPessoaRepository pessoaRepository)
    {
        _pessoaRepository = pessoaRepository;
    }

    #endregion

    public async Task<ObterPessoaPorIdView> Handle(ObterPessoaPorIdQuery request, CancellationToken cancellationToken)
    {
        var pessoa = await _pessoaRepository.ObterQueryable()
            .Where(p => p.Id == request.Id)
            .Select(p => new PessoaView
            {
                Id = p.Id,
                Nome = p.Nome.Valor,
                Cpf = p.Cpf.Valor,
                DataNascimento = p.DataNascimento,
                Idade = p.Idade
            })
            .FirstOrDefaultAsync(cancellationToken);

        return new ObterPessoaPorIdView(pessoa);
    }
}
