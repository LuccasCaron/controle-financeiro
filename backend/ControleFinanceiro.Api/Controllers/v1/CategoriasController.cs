using ControleFinanceiro.Api.DTOs.Categoria;
using ControleFinanceiro.Api.Helpers;
using ControleFinanceiro.Application.Categorias.Commands;
using ControleFinanceiro.Application.Categorias.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace ControleFinanceiro.Api.Controllers.v1;

[ApiController]
[Route("api/v1/[controller]")]
public class CategoriasController : ControllerBase
{
    #region Properties

    private readonly IMediator _mediator;

    #endregion

    #region Constructor

    public CategoriasController(IMediator mediator)
    {
        _mediator = mediator;
    }

    #endregion

    [HttpGet]
    public async Task<IActionResult> Listar([FromQuery] int page = 1, [FromQuery] int pageSize = 10, CancellationToken cancellationToken = default)
    {
        var (normalizedPage, normalizedPageSize) = PaginationHelper.NormalizePagination(page, pageSize);
        var query = new ListarCategoriasQuery(normalizedPage, normalizedPageSize);
        var response = await _mediator.Send(query, cancellationToken);
        return StatusCode(response.StatusCode, response);
    }

    [HttpPost]
    public async Task<IActionResult> Criar([FromBody] CriarCategoriaDto dto, CancellationToken cancellationToken)
    {
        var command = new CriarCategoriaCommand(dto.Nome, dto.Descricao, dto.Finalidade);
        var response = await _mediator.Send(command, cancellationToken);
        return StatusCode(response.StatusCode, response);
    }
}
