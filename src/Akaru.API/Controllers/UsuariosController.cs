using System.Security.Claims;
using Akaru.API.Extensions;
using Akaru.Application.DTOs;
using Akaru.Application.Interfaces;
using Akaru.Application.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Akaru.API.Controllers;

[ApiController]
[Route("api/usuarios")]
public class UsuariosController : ControllerBase
{
    private readonly UsuarioService _usuarioService;

    public UsuariosController(UsuarioService usuarioService)
    {
        _usuarioService = usuarioService;
    }

    /// <summary>
    /// Sincroniza o usuário autenticado via Firebase com o banco Oracle.
    /// Chamado pelo mobile após login/cadastro no Firebase.
    /// </summary>
    [HttpPost("sync")]
    [Authorize]
    public async Task<ActionResult<UsuarioResponseDto>> Sync(CancellationToken ct)
    {
        var firebaseUser = ObterUsuarioFirebase();
        var usuario = await _usuarioService.SincronizarAsync(firebaseUser, ct);
        return Ok(usuario);
    }

    [HttpGet("me")]
    [Authorize]
    public async Task<ActionResult<UsuarioResponseDto>> ObterPerfil(CancellationToken ct)
    {
        var usuarioId = await ObterUsuarioIdAsync(ct);
        var perfil = await _usuarioService.ObterPerfilAsync(usuarioId, ct);
        return Ok(perfil);
    }

    [HttpPut("me")]
    [Authorize]
    public async Task<ActionResult<UsuarioResponseDto>> AtualizarPerfil(
        [FromBody] AtualizarUsuarioDto dto,
        CancellationToken ct)
    {
        var usuarioId = await ObterUsuarioIdAsync(ct);
        var perfil = await _usuarioService.AtualizarPerfilAsync(usuarioId, dto, ct);
        return Ok(perfil);
    }

    private async Task<int> ObterUsuarioIdAsync(CancellationToken ct)
    {
        var firebaseUid = User.ObterFirebaseUid();
        return await _usuarioService.ObterIdPorFirebaseUidAsync(firebaseUid, ct);
    }

    private FirebaseUserInfo ObterUsuarioFirebase() =>
        new(
            User.ObterFirebaseUid(),
            User.FindFirstValue(ClaimTypes.Email),
            User.FindFirstValue(ClaimTypes.Name));
}
