using ControleFinanceiro.Api.DTOs.Pessoa;
using ControleFinanceiro.Api.Helpers;
using ControleFinanceiro.Application.Pessoas.Commands;
using ControleFinanceiro.Application.Pessoas.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace ControleFinanceiro.Api.Controllers.v1;

[ApiController]
[Route("api/v1/[controller]")]
public class PessoasController : ControllerBase
{
    #region Properties

    private readonly IMediator _mediator;

    #endregion

    #region Constructor

    public PessoasController(IMediator mediator)
    {
        _mediator = mediator;
    }

    #endregion

    [HttpGet]
    public async Task<IActionResult> Listar([FromQuery] int page = 1, [FromQuery] int pageSize = 10, CancellationToken cancellationToken = default)
    {
        var (normalizedPage, normalizedPageSize) = PaginationHelper.NormalizePagination(page, pageSize);
        var query = new ListarPessoasQuery(normalizedPage, normalizedPageSize);
        var response = await _mediator.Send(query, cancellationToken);
        return StatusCode(response.StatusCode, response);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> ObterPorId(Guid id, CancellationToken cancellationToken)
    {
        var query = new ObterPessoaPorIdQuery(id);
        var response = await _mediator.Send(query, cancellationToken);
        return StatusCode(response.StatusCode, response);
    }

    [HttpPost]
    public async Task<IActionResult> Criar([FromBody] CriarPessoaDto dto, CancellationToken cancellationToken)
    {
        var dataNascimentoUtc = dto.DataNascimento.Kind == DateTimeKind.Unspecified
            ? DateTime.SpecifyKind(dto.DataNascimento, DateTimeKind.Utc)
            : dto.DataNascimento.ToUniversalTime();
        
        var command = new CriarPessoaCommand(dto.Nome, dto.Cpf, dataNascimentoUtc);
        var response = await _mediator.Send(command, cancellationToken);
        return StatusCode(response.StatusCode, response);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Atualizar(Guid id, [FromBody] AtualizarPessoaDto dto, CancellationToken cancellationToken)
    {
        var dataNascimentoUtc = dto.DataNascimento.Kind == DateTimeKind.Unspecified
            ? DateTime.SpecifyKind(dto.DataNascimento, DateTimeKind.Utc)
            : dto.DataNascimento.ToUniversalTime();
        
        var command = new AtualizarPessoaCommand(id, dto.Nome, dataNascimentoUtc);
        var response = await _mediator.Send(command, cancellationToken);
        return StatusCode(response.StatusCode, response);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Excluir(Guid id, CancellationToken cancellationToken)
    {
        var command = new ExcluirPessoaCommand(id);
        var response = await _mediator.Send(command, cancellationToken);
        return StatusCode(response.StatusCode, response);
    }
}
