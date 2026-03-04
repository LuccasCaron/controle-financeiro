using ControleFinanceiro.Application.Categorias.Commands;
using ControleFinanceiro.Application.Categorias.Commands.Views;
using ControleFinanceiro.Application.Common;
using ControleFinanceiro.Domain.Entities;
using ControleFinanceiro.Domain.Repositories;
using ControleFinanceiro.Domain.ValueObjects;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace ControleFinanceiro.Application.Categorias.Commands.Handlers;

public class CriarCategoriaCommandHandler : IRequestHandler<CriarCategoriaCommand, BaseView>
{
    #region Properties

    private readonly ICategoriaRepository _categoriaRepository;

    #endregion

    #region Constructor

    public CriarCategoriaCommandHandler(ICategoriaRepository categoriaRepository)
    {
        _categoriaRepository = categoriaRepository;
    }

    #endregion

    public async Task<BaseView> Handle(CriarCategoriaCommand request, CancellationToken cancellationToken)
    {
        var categoriaExistente = await _categoriaRepository.ObterQueryable()
            .FirstOrDefaultAsync(c => c.Nome.Valor == request.Nome, cancellationToken);

        if (categoriaExistente != null)
        {
            return new ErrorView("Já existe uma categoria com este nome.", 400);
        }

        var nome = new Nome(request.Nome);
        var descricao = new Descricao(request.Descricao);
        var categoria = new Categoria(nome, descricao, request.Finalidade);

        _categoriaRepository.Adicionar(categoria);
        await _categoriaRepository.UnitOfWork.Commit();

        return new CriarCategoriaView(categoria.Id);
    }
}
