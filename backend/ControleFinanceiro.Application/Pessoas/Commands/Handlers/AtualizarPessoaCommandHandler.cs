using ControleFinanceiro.Application.Common;
using ControleFinanceiro.Application.Pessoas.Commands;
using ControleFinanceiro.Application.Pessoas.Commands.Views;
using ControleFinanceiro.Domain.Repositories;
using ControleFinanceiro.Domain.ValueObjects;
using MediatR;

namespace ControleFinanceiro.Application.Pessoas.Handlers;

public class AtualizarPessoaCommandHandler : IRequestHandler<AtualizarPessoaCommand, BaseView>
{
    #region Properties

    private readonly IPessoaRepository _pessoaRepository;

    #endregion

    #region Constructor

    public AtualizarPessoaCommandHandler(IPessoaRepository pessoaRepository)
    {
        _pessoaRepository = pessoaRepository;
    }

    #endregion

    public async Task<BaseView> Handle(AtualizarPessoaCommand request, CancellationToken cancellationToken)
    {
        var pessoa = await _pessoaRepository.ObterPorIdAsync(request.Id, cancellationToken);

        if (pessoa == null)
        {
            return new ErrorView("Pessoa não encontrada.", 404);
        }

        var nome = new Nome(request.Nome);
        pessoa.AtualizarNome(nome);
        pessoa.AtualizarDataNascimento(request.DataNascimento);

        await _pessoaRepository.UnitOfWork.Commit();

        return new AtualizarPessoaView();
    }
}
