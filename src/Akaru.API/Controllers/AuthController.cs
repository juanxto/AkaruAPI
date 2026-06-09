using Akaru.Application.DTOs;
using Akaru.Application.Services;
using Microsoft.AspNetCore.Mvc;

namespace Akaru.API.Controllers;

[ApiController]
[Route("api/auth")]
public class AuthController : ControllerBase
{
    private readonly AuthService _authService;

    public AuthController(AuthService authService)
    {
        _authService = authService;
    }

    /// <summary>Cadastra um novo agricultor e retorna JWT.</summary>
    [HttpPost("register")]
    public async Task<ActionResult<AuthResponseDto>> Registrar([FromBody] RegisterDto dto, CancellationToken ct)
    {
        var resposta = await _authService.RegistrarAsync(dto, ct);
        return CreatedAtAction(nameof(Registrar), resposta);
    }

    /// <summary>Autentica com e-mail e senha e retorna JWT.</summary>
    [HttpPost("login")]
    public async Task<ActionResult<AuthResponseDto>> Login([FromBody] LoginDto dto, CancellationToken ct)
    {
        var resposta = await _authService.LoginAsync(dto, ct);
        return Ok(resposta);
    }
}
