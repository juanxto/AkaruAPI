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
    private readonly UsuarioService _usuarioService;

    public PlantiosController(PlantioService plantioService, UsuarioService usuarioService)
    {
        _plantioService = plantioService;
        _usuarioService = usuarioService;
    }

    [HttpPost]
    public async Task<ActionResult<PlantioResponseDto>> Criar(
        [FromBody] CriarPlantioDto dto,
        CancellationToken ct)
    {
        var usuarioId = await ObterUsuarioIdAsync(ct);
        var plantio = await _plantioService.CriarAsync(usuarioId, dto, ct);
        return CreatedAtAction(nameof(ObterPorId), new { id = plantio.Id }, plantio);
    }

    [HttpGet]
    public async Task<ActionResult<IReadOnlyList<PlantioResponseDto>>> Listar(CancellationToken ct)
    {
        var usuarioId = await ObterUsuarioIdAsync(ct);
        var plantios = await _plantioService.ListarPorUsuarioAsync(usuarioId, ct);
        return Ok(plantios);
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<PlantioResponseDto>> ObterPorId(int id, CancellationToken ct)
    {
        var usuarioId = await ObterUsuarioIdAsync(ct);
        var plantio = await _plantioService.ObterPorIdAsync(usuarioId, id, ct);
        return Ok(plantio);
    }

    [HttpPut("{id:int}")]
    public async Task<ActionResult<PlantioResponseDto>> Atualizar(
        int id,
        [FromBody] AtualizarPlantioDto dto,
        CancellationToken ct)
    {
        var usuarioId = await ObterUsuarioIdAsync(ct);
        var plantio = await _plantioService.AtualizarAsync(usuarioId, id, dto, ct);
        return Ok(plantio);
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Remover(int id, CancellationToken ct)
    {
        var usuarioId = await ObterUsuarioIdAsync(ct);
        await _plantioService.RemoverAsync(usuarioId, id, ct);
        return NoContent();
    }

    private async Task<int> ObterUsuarioIdAsync(CancellationToken ct)
    {
        var firebaseUid = User.ObterFirebaseUid();
        return await _usuarioService.ObterIdPorFirebaseUidAsync(firebaseUid, ct);
    }
}
