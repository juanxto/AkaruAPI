using Akaru.Application.DTOs;
using Akaru.Application.Interfaces;
using Akaru.Domain.Entities;
using Akaru.Domain.Exceptions;

namespace Akaru.Application.Services;

public class PlantioService
{
    private readonly IPlantioRepository _repository;

    public PlantioService(IPlantioRepository repository)
    {
        _repository = repository;
    }

    public async Task<PlantioResponseDto> CriarAsync(int usuarioId, CriarPlantioDto dto, CancellationToken ct = default)
    {
        ValidarCoordenadas(dto.Latitude, dto.Longitude);

        var plantio = new Plantio
        {
            UsuarioId = usuarioId,
            CulturaId = dto.CulturaId,
            Latitude = dto.Latitude,
            Longitude = dto.Longitude,
            Cidade = dto.Cidade,
            Estado = dto.Estado,
            DataPlantio = dto.DataPlantio,
            Detalhes = dto.Detalhes,
            DataRegistro = DateTime.UtcNow
        };

        AssociarCulturas(plantio, dto.CulturaId, dto.CulturasAdicionais);

        var criado = await _repository.CriarAsync(plantio, ct);
        return Mapear(criado);
    }

    public async Task<IReadOnlyList<PlantioResponseDto>> ListarPorUsuarioAsync(int usuarioId, CancellationToken ct = default)
    {
        var plantios = await _repository.ListarPorUsuarioAsync(usuarioId, ct);
        return plantios.Select(Mapear).ToList();
    }

    public async Task<PlantioResponseDto> ObterPorIdAsync(int usuarioId, int plantioId, CancellationToken ct = default)
    {
        var plantio = await ObterPlantioDoUsuarioAsync(usuarioId, plantioId, ct);
        return Mapear(plantio);
    }

    public async Task<PlantioResponseDto> AtualizarAsync(int usuarioId, int plantioId, AtualizarPlantioDto dto, CancellationToken ct = default)
    {
        var plantio = await ObterPlantioDoUsuarioAsync(usuarioId, plantioId, ct);

        if (dto.CulturaId.HasValue)
            plantio.CulturaId = dto.CulturaId.Value;

        if (dto.Latitude.HasValue && dto.Longitude.HasValue)
            ValidarCoordenadas(dto.Latitude.Value, dto.Longitude.Value);

        if (dto.Latitude.HasValue)
            plantio.Latitude = dto.Latitude.Value;

        if (dto.Longitude.HasValue)
            plantio.Longitude = dto.Longitude.Value;

        if (dto.DataPlantio.HasValue)
            plantio.DataPlantio = dto.DataPlantio.Value;

        if (dto.Detalhes is not null)
            plantio.Detalhes = dto.Detalhes;

        if (dto.Cidade is not null)
            plantio.Cidade = dto.Cidade;

        if (dto.Estado is not null)
            plantio.Estado = dto.Estado;

        if (dto.CulturasAdicionais is not null)
        {
            plantio.PlantioCulturas.Clear();
            AssociarCulturas(plantio, plantio.CulturaId, dto.CulturasAdicionais);
        }

        await _repository.AtualizarAsync(plantio, ct);
        return Mapear(plantio);
    }

    public async Task RemoverAsync(int usuarioId, int plantioId, CancellationToken ct = default)
    {
        var plantio = await ObterPlantioDoUsuarioAsync(usuarioId, plantioId, ct);
        await _repository.RemoverAsync(plantio, ct);
    }

    private async Task<Plantio> ObterPlantioDoUsuarioAsync(int usuarioId, int plantioId, CancellationToken ct)
    {
        var plantio = await _repository.ObterPorIdAsync(plantioId, ct)
            ?? throw new NotFoundException("Plantio não encontrado.");

        if (plantio.UsuarioId != usuarioId)
            throw new AcessoNegadoException("Você não tem permissão para acessar este plantio.");

        return plantio;
    }

    private static void ValidarCoordenadas(decimal latitude, decimal longitude)
    {
        if (latitude is < -90 or > 90)
            throw new DomainException("Latitude inválida. Deve estar entre -90 e 90.");

        if (longitude is < -180 or > 180)
            throw new DomainException("Longitude inválida. Deve estar entre -180 e 180.");
    }

    private static void AssociarCulturas(Plantio plantio, int culturaPrincipal, List<int>? culturasAdicionais)
    {
        var culturas = new HashSet<int> { culturaPrincipal };
        if (culturasAdicionais is not null)
        {
            foreach (var culturaId in culturasAdicionais)
                culturas.Add(culturaId);
        }

        foreach (var culturaId in culturas)
        {
            plantio.PlantioCulturas.Add(new PlantioCultura
            {
                PlantioId = plantio.Id,
                CulturaId = culturaId
            });
        }
    }

    private static PlantioResponseDto Mapear(Plantio plantio) =>
        new(
            plantio.Id,
            plantio.CulturaId,
            plantio.Latitude,
            plantio.Longitude,
            plantio.Cidade,
            plantio.Estado,
            plantio.DataPlantio,
            plantio.Detalhes,
            plantio.DataRegistro,
            plantio.PlantioCulturas.Select(pc => pc.CulturaId).Distinct().ToList());
}
