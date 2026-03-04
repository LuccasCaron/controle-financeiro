using ControleFinanceiro.Api.DTOs.Transacao;
using ControleFinanceiro.Api.Helpers;
using ControleFinanceiro.Application.Transacoes.Commands;
using ControleFinanceiro.Application.Transacoes.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace ControleFinanceiro.Api.Controllers.v1;

[ApiController]
[Route("api/v1/[controller]")]
public class TransacoesController : ControllerBase
{
    #region Properties

    private readonly IMediator _mediator;

    #endregion

    #region Constructor

    public TransacoesController(IMediator mediator)
    {
        _mediator = mediator;
    }

    #endregion

    [HttpGet]
    public async Task<IActionResult> Listar([FromQuery] int page = 1, [FromQuery] int pageSize = 10, CancellationToken cancellationToken = default)
    {
        var (normalizedPage, normalizedPageSize) = PaginationHelper.NormalizePagination(page, pageSize);
        var query = new ListarTransacoesQuery(normalizedPage, normalizedPageSize);
        var response = await _mediator.Send(query, cancellationToken);
        return StatusCode(response.StatusCode, response);
    }

    [HttpPost]
    public async Task<IActionResult> Criar([FromBody] CriarTransacaoDto dto, CancellationToken cancellationToken)
    {
        var command = new CriarTransacaoCommand(dto.PessoaId, dto.CategoriaId, dto.Tipo, dto.Valor, dto.Data, dto.Descricao);
        var response = await _mediator.Send(command, cancellationToken);
        return StatusCode(response.StatusCode, response);
    }
}
