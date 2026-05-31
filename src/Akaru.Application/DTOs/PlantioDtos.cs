namespace Akaru.Application.DTOs;

public record CriarPlantioDto(
    int CulturaId,
    decimal Latitude,
    decimal Longitude,
    DateTime DataPlantio,
    string? Detalhes,
    string? Cidade,
    string? Estado,
    List<int>? CulturasAdicionais);

public record AtualizarPlantioDto(
    int? CulturaId,
    decimal? Latitude,
    decimal? Longitude,
    DateTime? DataPlantio,
    string? Detalhes,
    string? Cidade,
    string? Estado,
    List<int>? CulturasAdicionais);

public record PlantioResponseDto(
    int Id,
    int CulturaId,
    decimal Latitude,
    decimal Longitude,
    string? Cidade,
    string? Estado,
    DateTime DataPlantio,
    string? Detalhes,
    DateTime DataRegistro,
    List<int> CulturasAssociadas);
