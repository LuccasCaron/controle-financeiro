using ControleFinanceiro.Application.Common;
using ControleFinanceiro.Application.Pessoas.Commands;
using ControleFinanceiro.Application.Pessoas.Commands.Views;
using ControleFinanceiro.Domain.Entities;
using ControleFinanceiro.Domain.Repositories;
using ControleFinanceiro.Domain.ValueObjects;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace ControleFinanceiro.Application.Pessoas.Handlers;

public class CriarPessoaCommandHandler : IRequestHandler<CriarPessoaCommand, BaseView>
{
    #region Properties

    private readonly IPessoaRepository _pessoaRepository;

    #endregion

    #region Constructor

    public CriarPessoaCommandHandler(IPessoaRepository pessoaRepository)
    {
        _pessoaRepository = pessoaRepository;
    }

    #endregion

    public async Task<BaseView> Handle(CriarPessoaCommand request, CancellationToken cancellationToken)
    {
        var pessoaExistente = await _pessoaRepository.ObterQueryable()
            .FirstOrDefaultAsync(p => p.Cpf.Valor == request.Cpf, cancellationToken);

        if (pessoaExistente != null)
        {
            return new ErrorView("Já existe uma pessoa cadastrada com este CPF.", 400);
        }

        var nome = new Nome(request.Nome);
        var cpf = new Cpf(request.Cpf);
        var pessoa = new Pessoa(nome, cpf, request.DataNascimento);

        _pessoaRepository.Adicionar(pessoa);
        await _pessoaRepository.UnitOfWork.Commit();

        return new CriarPessoaView(pessoa.Id);
    }
}
