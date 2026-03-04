using ControleFinanceiro.Application.Common;
using ControleFinanceiro.Application.Transacoes.Commands;
using ControleFinanceiro.Application.Transacoes.Commands.Handlers;
using ControleFinanceiro.Application.Transacoes.Commands.Views;
using ControleFinanceiro.Domain.Enums;
using ControleFinanceiro.Domain.Repositories;
using ControleFinanceiro.Infrastructure.Data;
using ControleFinanceiro.Infrastructure.Data.Repositories;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;

namespace ControleFinanceiro.Application.Tests.Handlers;

public class CriarTransacaoCommandHandlerTests
{
    #region Properties

    private ApplicationDbContext _context;
    private ITransacaoRepository _transacaoRepository;
    private IPessoaRepository _pessoaRepository;
    private ICategoriaRepository _categoriaRepository;
    private CriarTransacaoCommandHandler _handler;

    #endregion

    #region Constructor

    public CriarTransacaoCommandHandlerTests()
    {
        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;

        _context = new ApplicationDbContext(options);
        _transacaoRepository = new TransacaoRepository(_context);
        _pessoaRepository = new PessoaRepository(_context);
        _categoriaRepository = new CategoriaRepository(_context);
        _handler = new CriarTransacaoCommandHandler(
            _transacaoRepository,
            _pessoaRepository,
            _categoriaRepository);
    }

    #endregion

    [Fact]
    public async Task Deve_Criar_Transacao_Com_Sucesso()
    {
        var pessoa = CriarPessoaMaiorDeIdade();
        var categoria = CriarCategoriaDespesa();
        _context.Pessoas.Add(pessoa);
        _context.Categorias.Add(categoria);
        await _context.SaveChangesAsync();

        var command = new CriarTransacaoCommand(
            pessoa.Id,
            categoria.Id,
            TipoTransacao.Despesa,
            100m,
            DateTime.UtcNow.AddDays(-1),
            "Descrição da transação");

        var resultado = await _handler.Handle(command, CancellationToken.None);

        resultado.Should().BeOfType<CriarTransacaoView>();
        resultado.Success.Should().BeTrue();
        resultado.StatusCode.Should().Be(201);

        var transacaoCriada = await _context.Transacoes.FirstOrDefaultAsync();
        transacaoCriada.Should().NotBeNull();
        transacaoCriada!.Valor.Valor.Should().Be(100m);
    }

    [Fact]
    public async Task Deve_Validar_Regras_De_Negocio_Menor_De_Idade()
    {
        var pessoa = CriarPessoaMenorDeIdade();
        var categoria = CriarCategoriaDespesa();
        _context.Pessoas.Add(pessoa);
        _context.Categorias.Add(categoria);
        await _context.SaveChangesAsync();

        var command = new CriarTransacaoCommand(
            pessoa.Id,
            categoria.Id,
            TipoTransacao.Receita,
            100m,
            DateTime.UtcNow.AddDays(-1),
            "Descrição");

        var resultado = await _handler.Handle(command, CancellationToken.None);

        resultado.Should().BeOfType<ErrorView>();
        resultado.Success.Should().BeFalse();
        resultado.StatusCode.Should().Be(400);
        resultado.Message.Should().Contain("Menores de 18 anos só podem ter despesas");
    }

    [Fact]
    public async Task Deve_Validar_Categoria_Compativel()
    {
        var pessoa = CriarPessoaMaiorDeIdade();
        var categoria = CriarCategoriaDespesa();
        _context.Pessoas.Add(pessoa);
        _context.Categorias.Add(categoria);
        await _context.SaveChangesAsync();

        var command = new CriarTransacaoCommand(
            pessoa.Id,
            categoria.Id,
            TipoTransacao.Receita,
            100m,
            DateTime.UtcNow.AddDays(-1),
            "Descrição");

        var resultado = await _handler.Handle(command, CancellationToken.None);

        resultado.Should().BeOfType<ErrorView>();
        resultado.Success.Should().BeFalse();
        resultado.StatusCode.Should().Be(400);
        resultado.Message.Should().Contain("é exclusiva para despesas");
    }

    private Domain.Entities.Pessoa CriarPessoaMaiorDeIdade()
    {
        return new Domain.Entities.Pessoa(
            new Domain.ValueObjects.Nome("João Silva"),
            new Domain.ValueObjects.Cpf("11144477735"),
            DateTime.UtcNow.AddYears(-25));
    }

    private Domain.Entities.Pessoa CriarPessoaMenorDeIdade()
    {
        return new Domain.Entities.Pessoa(
            new Domain.ValueObjects.Nome("João Silva"),
            new Domain.ValueObjects.Cpf("11144477735"),
            DateTime.UtcNow.AddYears(-17));
    }

    private Domain.Entities.Categoria CriarCategoriaDespesa()
    {
        return new Domain.Entities.Categoria(
            new Domain.ValueObjects.Nome("Alimentação"),
            new Domain.ValueObjects.Descricao("Gastos com alimentação"),
            FinalidadeCategoria.Despesa);
    }
}
