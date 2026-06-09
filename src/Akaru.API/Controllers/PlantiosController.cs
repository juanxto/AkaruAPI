using Akaru.API.Extensions;
using Akaru.Application.DTOs;
using Akaru.Application.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Akaru.API.Controllers;

[ApiController]
[Route("api/plantios")]
[Authorize]
public class PlantiosController : ControllerBase
{
    private readonly PlantioService _plantioService;

    public PlantiosController(PlantioService plantioService)
    {
        _plantioService = plantioService;
    }

    [HttpPost]
    public async Task<ActionResult<PlantioResponseDto>> Criar(
        [FromBody] CriarPlantioDto dto,
        CancellationToken ct)
    {
        var usuarioId = ObterUsuarioId();
        var plantio = await _plantioService.CriarAsync(usuarioId, dto, ct);
        return CreatedAtAction(nameof(ObterPorId), new { id = plantio.Id }, plantio);
    }

    [HttpGet]
    public async Task<ActionResult<IReadOnlyList<PlantioResponseDto>>> Listar(CancellationToken ct)
    {
        var usuarioId = ObterUsuarioId();
        var plantios = await _plantioService.ListarPorUsuarioAsync(usuarioId, ct);
        return Ok(plantios);
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<PlantioResponseDto>> ObterPorId(int id, CancellationToken ct)
    {
        var usuarioId = ObterUsuarioId();
        var plantio = await _plantioService.ObterPorIdAsync(usuarioId, id, ct);
        return Ok(plantio);
    }

    [HttpPut("{id:int}")]
    public async Task<ActionResult<PlantioResponseDto>> Atualizar(
        int id,
        [FromBody] AtualizarPlantioDto dto,
        CancellationToken ct)
    {
        var usuarioId = ObterUsuarioId();
        var plantio = await _plantioService.AtualizarAsync(usuarioId, id, dto, ct);
        return Ok(plantio);
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Remover(int id, CancellationToken ct)
    {
        var usuarioId = ObterUsuarioId();
        await _plantioService.RemoverAsync(usuarioId, id, ct);
        return NoContent();
    }

    private int ObterUsuarioId() => User.ObterUsuarioId();
}
