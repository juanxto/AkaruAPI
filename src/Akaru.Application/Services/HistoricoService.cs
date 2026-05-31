using System.Text.Json;
using Akaru.Application.DTOs;
using Akaru.Application.Interfaces;
using Akaru.Domain.Entities;
using Akaru.Domain.Exceptions;

namespace Akaru.Application.Services;

public class HistoricoService
{
    private readonly IHistoricoRepository _repository;

    public HistoricoService(IHistoricoRepository repository)
    {
        _repository = repository;
    }

    public async Task<HistoricoResponseDto> SalvarAsync(int usuarioId, SalvarHistoricoDto dto, CancellationToken ct = default)
    {
        if (string.IsNullOrWhiteSpace(dto.TextoRecomendacao))
            throw new DomainException("Texto da recomendação é obrigatório.");

        var texto = dto.TextoRecomendacao;

        if (dto.RecomendacaoCompleta is not null && string.IsNullOrWhiteSpace(texto))
        {
            texto = JsonSerializer.Serialize(dto.RecomendacaoCompleta);
        }

        var historico = new HistoricoRecomendacao
        {
            UsuarioId = usuarioId,
            CulturaId = dto.CulturaId,
            CulturaNome = dto.CulturaNome,
            TextoRecomendacao = texto,
            Score = dto.Score,
            Latitude = dto.Latitude,
            Longitude = dto.Longitude,
            Detalhes = dto.Detalhes,
            DadosClimaticos = dto.DadosClimaticos,
            DataGeracao = DateTime.UtcNow
        };

        var criado = await _repository.CriarAsync(historico, ct);
        return Mapear(criado);
    }

    public async Task<IReadOnlyList<HistoricoResponseDto>> ListarPorUsuarioAsync(int usuarioId, CancellationToken ct = default)
    {
        var historicos = await _repository.ListarPorUsuarioAsync(usuarioId, ct);
        return historicos.Select(Mapear).ToList();
    }

    public async Task<HistoricoResponseDto> ObterPorIdAsync(int usuarioId, int historicoId, CancellationToken ct = default)
    {
        var historico = await _repository.ObterPorIdAsync(historicoId, ct)
            ?? throw new NotFoundException("Recomendação não encontrada no histórico.");

        if (historico.UsuarioId != usuarioId)
            throw new AcessoNegadoException("Você não tem permissão para acessar este histórico.");

        return Mapear(historico);
    }

    private static HistoricoResponseDto Mapear(HistoricoRecomendacao historico)
    {
        var resumo = historico.TextoRecomendacao.Length > 120
            ? historico.TextoRecomendacao[..117] + "..."
            : historico.TextoRecomendacao;

        return new HistoricoResponseDto(
            historico.Id,
            historico.CulturaId,
            historico.CulturaNome,
            historico.TextoRecomendacao,
            historico.Score,
            historico.Latitude,
            historico.Longitude,
            historico.Detalhes,
            historico.DadosClimaticos,
            historico.DataGeracao,
            resumo);
    }
}
