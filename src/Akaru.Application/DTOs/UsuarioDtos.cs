namespace Akaru.Application.DTOs;

public record UsuarioResponseDto(
    int Id,
    string Nome,
    string Email,
    decimal? Latitude,
    decimal? Longitude,
    string? Cidade,
    string? Estado,
    DateTime DataCadastro);

public record AtualizarUsuarioDto(
    string? Nome,
    decimal? Latitude,
    decimal? Longitude,
    string? Cidade,
    string? Estado);
