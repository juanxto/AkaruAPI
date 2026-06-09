using Akaru.API.Extensions;
using Akaru.Application.DTOs;
using Akaru.Application.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Akaru.API.Controllers;

[ApiController]
[Route("api/usuarios")]
[Authorize]
public class UsuariosController : ControllerBase
{
    private readonly UsuarioService _usuarioService;

    public UsuariosController(UsuarioService usuarioService)
    {
        _usuarioService = usuarioService;
    }

    [HttpGet("me")]
    public async Task<ActionResult<UsuarioResponseDto>> ObterPerfil(CancellationToken ct)
    {
        var usuarioId = User.ObterUsuarioId();
        var perfil = await _usuarioService.ObterPerfilAsync(usuarioId, ct);
        return Ok(perfil);
    }

    [HttpPut("me")]
    public async Task<ActionResult<UsuarioResponseDto>> AtualizarPerfil(
        [FromBody] AtualizarUsuarioDto dto,
        CancellationToken ct)
    {
        var usuarioId = User.ObterUsuarioId();
        var perfil = await _usuarioService.AtualizarPerfilAsync(usuarioId, dto, ct);
        return Ok(perfil);
    }
}
