using Akaru.API.Extensions;
using Akaru.Application.DTOs;
using Akaru.Application.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Akaru.API.Controllers;

[ApiController]
[Route("api/historico")]
[Authorize]
public class HistoricoController : ControllerBase
{
    private readonly HistoricoService _historicoService;

    public HistoricoController(HistoricoService historicoService)
    {
        _historicoService = historicoService;
    }

    [HttpPost]
    public async Task<ActionResult<HistoricoResponseDto>> Salvar(
        [FromBody] SalvarHistoricoDto dto,
        CancellationToken ct)
    {
        var usuarioId = ObterUsuarioId();
        var historico = await _historicoService.SalvarAsync(usuarioId, dto, ct);
        return CreatedAtAction(nameof(ObterPorId), new { id = historico.Id }, historico);
    }

    [HttpGet]
    public async Task<ActionResult<IReadOnlyList<HistoricoResponseDto>>> Listar(CancellationToken ct)
    {
        var usuarioId = ObterUsuarioId();
        var historicos = await _historicoService.ListarPorUsuarioAsync(usuarioId, ct);
        return Ok(historicos);
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<HistoricoResponseDto>> ObterPorId(int id, CancellationToken ct)
    {
        var usuarioId = ObterUsuarioId();
        var historico = await _historicoService.ObterPorIdAsync(usuarioId, id, ct);
        return Ok(historico);
    }

    private int ObterUsuarioId() => User.ObterUsuarioId();
}
