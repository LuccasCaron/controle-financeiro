using ControleFinanceiro.Application.Common;
using ControleFinanceiro.Application.Pessoas.Commands;
using ControleFinanceiro.Application.Pessoas.Commands.Views;
using ControleFinanceiro.Application.Pessoas.Handlers;
using ControleFinanceiro.Domain.Repositories;
using ControleFinanceiro.Infrastructure.Data;
using ControleFinanceiro.Infrastructure.Data.Repositories;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;

namespace ControleFinanceiro.Application.Tests.Handlers;

public class CriarPessoaCommandHandlerTests
{
    #region Properties

    private ApplicationDbContext _context;
    private IPessoaRepository _pessoaRepository;
    private CriarPessoaCommandHandler _handler;

    #endregion

    #region Constructor

    public CriarPessoaCommandHandlerTests()
    {
        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;

        _context = new ApplicationDbContext(options);
        _pessoaRepository = new PessoaRepository(_context);
        _handler = new CriarPessoaCommandHandler(_pessoaRepository);
    }

    #endregion

    [Fact]
    public async Task Deve_Criar_Pessoa_Com_Sucesso()
    {
        var command = new CriarPessoaCommand(
            "João Silva",
            "11144477735",
            DateTime.UtcNow.AddYears(-25));

        var resultado = await _handler.Handle(command, CancellationToken.None);

        resultado.Should().BeOfType<CriarPessoaView>();
        resultado.Success.Should().BeTrue();
        resultado.StatusCode.Should().Be(201);

        var pessoaCriada = await _context.Pessoas.FirstOrDefaultAsync();
        pessoaCriada.Should().NotBeNull();
        pessoaCriada!.Nome.Valor.Should().Be("João Silva");
        pessoaCriada.Cpf.Valor.Should().Be("11144477735");
    }

    [Fact]
    public async Task Deve_Retornar_Erro_Quando_Cpf_Ja_Existe()
    {
        var cpfExistente = "11144477735";
        var pessoaExistente = new Domain.Entities.Pessoa(
            new Domain.ValueObjects.Nome("Maria Santos"),
            new Domain.ValueObjects.Cpf(cpfExistente),
            DateTime.UtcNow.AddYears(-30));

        _context.Pessoas.Add(pessoaExistente);
        await _context.SaveChangesAsync();

        var command = new CriarPessoaCommand(
            "João Silva",
            cpfExistente,
            DateTime.UtcNow.AddYears(-25));

        var resultado = await _handler.Handle(command, CancellationToken.None);

        resultado.Should().BeOfType<ErrorView>();
        resultado.Success.Should().BeFalse();
        resultado.StatusCode.Should().Be(400);
        resultado.Message.Should().Contain("Já existe uma pessoa cadastrada com este CPF");
    }

    [Fact]
    public async Task Deve_Validar_Dados_Antes_De_Criar()
    {
        var command = new CriarPessoaCommand(
            "João Silva",
            "11144477735",
            DateTime.UtcNow.AddYears(-25));

        var resultado = await _handler.Handle(command, CancellationToken.None);

        resultado.Should().BeOfType<CriarPessoaView>();
        var view = resultado as CriarPessoaView;
        view!.Id.Should().NotBeEmpty();
    }
}
