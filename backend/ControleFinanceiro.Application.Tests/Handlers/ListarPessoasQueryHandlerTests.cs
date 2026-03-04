using ControleFinanceiro.Application.Pessoas.Handlers;
using ControleFinanceiro.Application.Pessoas.Queries;
using ControleFinanceiro.Domain.Repositories;
using ControleFinanceiro.Infrastructure.Data;
using ControleFinanceiro.Infrastructure.Data.Repositories;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;

namespace ControleFinanceiro.Application.Tests.Handlers;

public class ListarPessoasQueryHandlerTests
{
    #region Properties

    private ApplicationDbContext _context;
    private IPessoaRepository _pessoaRepository;
    private ListarPessoasQueryHandler _handler;

    #endregion

    #region Constructor

    public ListarPessoasQueryHandlerTests()
    {
        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;

        _context = new ApplicationDbContext(options);
        _pessoaRepository = new PessoaRepository(_context);
        _handler = new ListarPessoasQueryHandler(_pessoaRepository);
    }

    #endregion

    [Fact]
    public async Task Deve_Retornar_Lista_De_Pessoas()
    {
        var pessoa1 = new Domain.Entities.Pessoa(
            new Domain.ValueObjects.Nome("João Silva"),
            new Domain.ValueObjects.Cpf("11144477735"),
            DateTime.UtcNow.AddYears(-25));

        var pessoa2 = new Domain.Entities.Pessoa(
            new Domain.ValueObjects.Nome("Maria Santos"),
            new Domain.ValueObjects.Cpf("12345678909"),
            DateTime.UtcNow.AddYears(-30));

        _context.Pessoas.AddRange(pessoa1, pessoa2);
        await _context.SaveChangesAsync();

        var query = new ListarPessoasQuery();
        var resultado = await _handler.Handle(query, CancellationToken.None);

        resultado.Should().NotBeNull();
        resultado.Pessoas.Should().HaveCount(2);
        resultado.Pessoas.Should().Contain(p => p.Nome == "João Silva");
        resultado.Pessoas.Should().Contain(p => p.Nome == "Maria Santos");
    }

    [Fact]
    public async Task Deve_Retornar_Lista_Vazia_Quando_Nao_Ha_Pessoas()
    {
        var query = new ListarPessoasQuery();
        var resultado = await _handler.Handle(query, CancellationToken.None);

        resultado.Should().NotBeNull();
        resultado.Pessoas.Should().BeEmpty();
    }
}
