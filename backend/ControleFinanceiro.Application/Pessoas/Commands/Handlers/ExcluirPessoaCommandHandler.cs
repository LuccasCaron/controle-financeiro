using ControleFinanceiro.Application.Common;
using ControleFinanceiro.Application.Pessoas.Commands;
using ControleFinanceiro.Application.Pessoas.Commands.Views;
using ControleFinanceiro.Domain.Repositories;
using MediatR;

namespace ControleFinanceiro.Application.Pessoas.Handlers;

public class ExcluirPessoaCommandHandler : IRequestHandler<ExcluirPessoaCommand, BaseView>
{
    #region Properties

    private readonly IPessoaRepository _pessoaRepository;

    #endregion

    #region Constructor

    public ExcluirPessoaCommandHandler(IPessoaRepository pessoaRepository)
    {
        _pessoaRepository = pessoaRepository;
    }

    #endregion

    public async Task<BaseView> Handle(ExcluirPessoaCommand request, CancellationToken cancellationToken)
    {
        var pessoa = await _pessoaRepository.ObterPorIdAsync(request.Id, cancellationToken);

        if (pessoa == null)
        {
            return new ErrorView("Pessoa não encontrada.", 404);
        }

        _pessoaRepository.Remover(pessoa);
        await _pessoaRepository.UnitOfWork.Commit();

        return new ExcluirPessoaView();
    }
}
