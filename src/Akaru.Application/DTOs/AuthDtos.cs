namespace Akaru.Application.DTOs;

public record LoginDto(string Email, string Senha);

public record RegisterDto(string Nome, string Email, string Senha);

public record AuthResponseDto(string Token, DateTime ExpiraEm, UsuarioResponseDto Usuario);
