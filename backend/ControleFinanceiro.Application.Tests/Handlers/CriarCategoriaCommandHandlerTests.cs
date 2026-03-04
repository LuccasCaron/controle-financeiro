using ControleFinanceiro.Application.Categorias.Commands;
using ControleFinanceiro.Application.Categorias.Commands.Handlers;
using ControleFinanceiro.Application.Categorias.Commands.Views;
using ControleFinanceiro.Application.Common;
using ControleFinanceiro.Domain.Enums;
using ControleFinanceiro.Domain.Repositories;
using ControleFinanceiro.Infrastructure.Data;
using ControleFinanceiro.Infrastructure.Data.Repositories;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;

namespace ControleFinanceiro.Application.Tests.Handlers;

public class CriarCategoriaCommandHandlerTests
{
    #region Properties

    private ApplicationDbContext _context;
    private ICategoriaRepository _categoriaRepository;
    private CriarCategoriaCommandHandler _handler;

    #endregion

    #region Constructor

    public CriarCategoriaCommandHandlerTests()
    {
        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;

        _context = new ApplicationDbContext(options);
        _categoriaRepository = new CategoriaRepository(_context);
        _handler = new CriarCategoriaCommandHandler(_categoriaRepository);
    }

    #endregion

    [Fact]
    public async Task Deve_Criar_Categoria_Com_Sucesso()
    {
        var command = new CriarCategoriaCommand(
            "Alimentação",
            "Gastos com alimentação",
            FinalidadeCategoria.Despesa);

        var resultado = await _handler.Handle(command, CancellationToken.None);

        resultado.Should().BeOfType<CriarCategoriaView>();
        resultado.Success.Should().BeTrue();
        resultado.StatusCode.Should().Be(201);

        var categoriaCriada = await _context.Categorias.FirstOrDefaultAsync();
        categoriaCriada.Should().NotBeNull();
        categoriaCriada!.Nome.Valor.Should().Be("Alimentação");
        categoriaCriada.Descricao.Valor.Should().Be("Gastos com alimentação");
        categoriaCriada.Finalidade.Should().Be(FinalidadeCategoria.Despesa);
    }

    [Fact]
    public async Task Deve_Retornar_Erro_Quando_Nome_Ja_Existe()
    {
        var nomeExistente = "Alimentação";
        var categoriaExistente = new Domain.Entities.Categoria(
            new Domain.ValueObjects.Nome(nomeExistente),
            new Domain.ValueObjects.Descricao("Gastos com alimentação"),
            FinalidadeCategoria.Despesa);

        _context.Categorias.Add(categoriaExistente);
        await _context.SaveChangesAsync();

        var command = new CriarCategoriaCommand(
            nomeExistente,
            "Nova descrição",
            FinalidadeCategoria.Receita);

        var resultado = await _handler.Handle(command, CancellationToken.None);

        resultado.Should().BeOfType<ErrorView>();
        resultado.Success.Should().BeFalse();
        resultado.StatusCode.Should().Be(400);
        resultado.Message.Should().Contain("Já existe uma categoria com este nome");
    }
}
