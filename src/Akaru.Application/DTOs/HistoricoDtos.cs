namespace Akaru.Application.DTOs;

public record SalvarHistoricoDto(
    int? CulturaId,
    string? CulturaNome,
    string TextoRecomendacao,
    decimal? Score,
    decimal? Latitude,
    decimal? Longitude,
    string? Detalhes,
    string? DadosClimaticos,
    object? RecomendacaoCompleta);

public record HistoricoResponseDto(
    int Id,
    int? CulturaId,
    string? CulturaNome,
    string TextoRecomendacao,
    decimal? Score,
    decimal? Latitude,
    decimal? Longitude,
    string? Detalhes,
    string? DadosClimaticos,
    DateTime DataGeracao,
    string? Resumo);
