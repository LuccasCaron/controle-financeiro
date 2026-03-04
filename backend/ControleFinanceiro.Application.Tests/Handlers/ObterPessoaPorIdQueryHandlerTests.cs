using ControleFinanceiro.Application.Pessoas.Handlers;
using ControleFinanceiro.Application.Pessoas.Queries;
using ControleFinanceiro.Domain.Repositories;
using ControleFinanceiro.Infrastructure.Data;
using ControleFinanceiro.Infrastructure.Data.Repositories;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;

namespace ControleFinanceiro.Application.Tests.Handlers;

public class ObterPessoaPorIdQueryHandlerTests
{
    #region Properties

    private ApplicationDbContext _context;
    private IPessoaRepository _pessoaRepository;
    private ObterPessoaPorIdQueryHandler _handler;

    #endregion

    #region Constructor

    public ObterPessoaPorIdQueryHandlerTests()
    {
        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;

        _context = new ApplicationDbContext(options);
        _pessoaRepository = new PessoaRepository(_context);
        _handler = new ObterPessoaPorIdQueryHandler(_pessoaRepository);
    }

    #endregion

    [Fact]
    public async Task Deve_Retornar_Pessoa_Quando_Existe()
    {
        var pessoa = new Domain.Entities.Pessoa(
            new Domain.ValueObjects.Nome("João Silva"),
            new Domain.ValueObjects.Cpf("11144477735"),
            DateTime.UtcNow.AddYears(-25));

        _context.Pessoas.Add(pessoa);
        await _context.SaveChangesAsync();

        var query = new ObterPessoaPorIdQuery(pessoa.Id);
        var resultado = await _handler.Handle(query, CancellationToken.None);

        resultado.Should().NotBeNull();
        resultado.Pessoa.Should().NotBeNull();
        resultado.Pessoa!.Nome.Should().Be("João Silva");
        resultado.Pessoa.Cpf.Should().Be("11144477735");
    }

    [Fact]
    public async Task Deve_Retornar_Null_Quando_Pessoa_Nao_Existe()
    {
        var query = new ObterPessoaPorIdQuery(Guid.NewGuid());
        var resultado = await _handler.Handle(query, CancellationToken.None);

        resultado.Should().NotBeNull();
        resultado.Pessoa.Should().BeNull();
    }
}
