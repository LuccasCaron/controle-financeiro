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

public class ExcluirPessoaCommandHandlerTests
{
    #region Properties

    private ApplicationDbContext _context;
    private IPessoaRepository _pessoaRepository;
    private ExcluirPessoaCommandHandler _handler;

    #endregion

    #region Constructor

    public ExcluirPessoaCommandHandlerTests()
    {
        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;

        _context = new ApplicationDbContext(options);
        _pessoaRepository = new PessoaRepository(_context);
        _handler = new ExcluirPessoaCommandHandler(_pessoaRepository);
    }

    #endregion

    [Fact]
    public async Task Deve_Excluir_Pessoa_Existente()
    {
        var pessoa = new Domain.Entities.Pessoa(
            new Domain.ValueObjects.Nome("João Silva"),
            new Domain.ValueObjects.Cpf("11144477735"),
            DateTime.UtcNow.AddYears(-25));

        _context.Pessoas.Add(pessoa);
        await _context.SaveChangesAsync();

        var command = new ExcluirPessoaCommand(pessoa.Id);

        var resultado = await _handler.Handle(command, CancellationToken.None);

        resultado.Should().BeOfType<ExcluirPessoaView>();
        resultado.Success.Should().BeTrue();
        resultado.StatusCode.Should().Be(200);

        var pessoaExcluida = await _context.Pessoas.FindAsync(pessoa.Id);
        pessoaExcluida.Should().BeNull();
    }

    [Fact]
    public async Task Deve_Retornar_Erro_Quando_Pessoa_Nao_Existe()
    {
        var command = new ExcluirPessoaCommand(Guid.NewGuid());

        var resultado = await _handler.Handle(command, CancellationToken.None);

        resultado.Should().BeOfType<ErrorView>();
        resultado.Success.Should().BeFalse();
        resultado.StatusCode.Should().Be(404);
        resultado.Message.Should().Contain("Pessoa não encontrada");
    }
}
