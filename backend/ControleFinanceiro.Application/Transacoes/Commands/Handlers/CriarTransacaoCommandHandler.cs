using ControleFinanceiro.Application.Common;
using ControleFinanceiro.Application.Transacoes.Commands.Views;
using ControleFinanceiro.Domain.Entities;
using ControleFinanceiro.Domain.Enums;
using ControleFinanceiro.Domain.Repositories;
using ControleFinanceiro.Domain.ValueObjects;
using MediatR;

namespace ControleFinanceiro.Application.Transacoes.Commands.Handlers;

public class CriarTransacaoCommandHandler : IRequestHandler<CriarTransacaoCommand, BaseView>
{
    #region Properties

    private readonly ITransacaoRepository _transacaoRepository;
    private readonly IPessoaRepository _pessoaRepository;
    private readonly ICategoriaRepository _categoriaRepository;

    #endregion

    #region Constructor

    public CriarTransacaoCommandHandler(
        ITransacaoRepository transacaoRepository,
        IPessoaRepository pessoaRepository,
        ICategoriaRepository categoriaRepository)
    {
        _transacaoRepository = transacaoRepository;
        _pessoaRepository = pessoaRepository;
        _categoriaRepository = categoriaRepository;
    }

    #endregion

    public async Task<BaseView> Handle(CriarTransacaoCommand request, CancellationToken cancellationToken)
    {
        var pessoa = await _pessoaRepository.ObterPorIdAsync(request.PessoaId, cancellationToken);

        if (pessoa == null)
        {
            return new ErrorView("Pessoa não encontrada.", 404);
        }

        var categoria = await _categoriaRepository.ObterPorIdAsync(request.CategoriaId, cancellationToken);

        if (categoria == null)
        {
            return new ErrorView("Categoria não encontrada.", 404);
        }

        var validacaoPessoa = ValidarTipoTransacaoParaPessoa(pessoa, request.Tipo);
        if (validacaoPessoa != null)
        {
            return validacaoPessoa;
        }

        var validacaoCategoria = ValidarCategoriaCompativel(categoria, request.Tipo);
        if (validacaoCategoria != null)
        {
            return validacaoCategoria;
        }

        var validacaoData = ValidarDataTransacao(request.Data);
        if (validacaoData != null)
        {
            return validacaoData;
        }

        var valor = new Dinheiro(request.Valor);
        var descricao = new Descricao(request.Descricao);

        var transacao = new Transacao(pessoa, categoria, request.Tipo, valor, request.Data, descricao);

        _transacaoRepository.Adicionar(transacao);
        await _transacaoRepository.UnitOfWork.Commit();

        return new CriarTransacaoView(transacao.Id);
    }

    #region Private Methods

    private static ErrorView? ValidarTipoTransacaoParaPessoa(Pessoa pessoa, TipoTransacao tipoTransacao)
    {
        if (pessoa.EMenorDeIdade && tipoTransacao == TipoTransacao.Receita)
        {
            return new ErrorView("Menores de 18 anos só podem ter despesas.", 400);
        }

        return null;
    }

    private static ErrorView? ValidarCategoriaCompativel(Categoria categoria, TipoTransacao tipoTransacao)
    {
        var categoriaCompativel = categoria.Finalidade switch
        {
            FinalidadeCategoria.Receita => tipoTransacao == TipoTransacao.Receita,
            FinalidadeCategoria.Despesa => tipoTransacao == TipoTransacao.Despesa,
            FinalidadeCategoria.Ambas => true,
            _ => false
        };

        if (!categoriaCompativel)
        {
            var tipoTransacaoTexto = tipoTransacao == TipoTransacao.Receita ? "receita" : "despesa";
            var finalidadeTexto = categoria.Finalidade switch
            {
                FinalidadeCategoria.Receita => "receitas",
                FinalidadeCategoria.Despesa => "despesas",
                _ => "ambas"
            };

            return new ErrorView(
                $"A categoria '{categoria.Nome.Valor}' é exclusiva para {finalidadeTexto} e não pode ser usada para {tipoTransacaoTexto}.",
                400);
        }

        return null;
    }

    private static ErrorView? ValidarDataTransacao(DateTime data)
    {
        if (data > DateTime.UtcNow)
        {
            return new ErrorView("A data da transação não pode ser no futuro.", 400);
        }

        return null;
    }

    #endregion
}
