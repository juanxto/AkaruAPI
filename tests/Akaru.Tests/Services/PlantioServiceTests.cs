using Akaru.Application.DTOs;
using Akaru.Application.Interfaces;
using Akaru.Application.Services;
using Akaru.Domain.Entities;
using Akaru.Domain.Exceptions;
using Moq;
using Xunit;

namespace Akaru.Tests.Services;

public class PlantioServiceTests
{
    private readonly Mock<IPlantioRepository> _repositoryMock;
    private readonly PlantioService _service;

    public PlantioServiceTests()
    {
        _repositoryMock = new Mock<IPlantioRepository>();
        _service = new PlantioService(_repositoryMock.Object);
    }

    [Fact]
    public async Task CriarAsync_DeveRetornarPlantio_QuandoDadosValidos()
    {
        // Arrange
        var dto = new CriarPlantioDto(
            CulturaId: 1,
            Latitude: -23.5505m,
            Longitude: -46.6333m,
            DataPlantio: DateTime.UtcNow,
            Detalhes: "Solo argiloso",
            Cidade: "São Paulo",
            Estado: "SP",
            CulturasAdicionais: new List<int> { 2 });

        _repositoryMock
            .Setup(r => r.CriarAsync(It.IsAny<Plantio>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync((Plantio p, CancellationToken _) =>
            {
                p.Id = 10;
                return p;
            });

        // Act
        var resultado = await _service.CriarAsync(usuarioId: 1, dto);

        // Assert
        Assert.Equal(10, resultado.Id);
        Assert.Equal(1, resultado.CulturaId);
        Assert.Equal("São Paulo", resultado.Cidade);
        Assert.Contains(1, resultado.CulturasAssociadas);
        Assert.Contains(2, resultado.CulturasAssociadas);
        _repositoryMock.Verify(r => r.CriarAsync(It.IsAny<Plantio>(), It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task CriarAsync_DeveLancarDomainException_QuandoLatitudeInvalida()
    {
        // Arrange
        var dto = new CriarPlantioDto(1, 95m, -46m, DateTime.UtcNow, null, null, null, null);

        // Act & Assert
        await Assert.ThrowsAsync<DomainException>(() => _service.CriarAsync(1, dto));
        _repositoryMock.Verify(r => r.CriarAsync(It.IsAny<Plantio>(), It.IsAny<CancellationToken>()), Times.Never);
    }

    [Fact]
    public async Task ObterPorIdAsync_DeveLancarAcessoNegado_QuandoPlantioDeOutroUsuario()
    {
        // Arrange
        _repositoryMock
            .Setup(r => r.ObterPorIdAsync(5, It.IsAny<CancellationToken>()))
            .ReturnsAsync(new Plantio { Id = 5, UsuarioId = 99, CulturaId = 1 });

        // Act & Assert
        await Assert.ThrowsAsync<AcessoNegadoException>(() => _service.ObterPorIdAsync(1, 5));
    }

    [Fact]
    public async Task RemoverAsync_DeveChamarRepositorio_QuandoPlantioPertenceAoUsuario()
    {
        // Arrange
        var plantio = new Plantio { Id = 3, UsuarioId = 1, CulturaId = 1 };
        _repositoryMock
            .Setup(r => r.ObterPorIdAsync(3, It.IsAny<CancellationToken>()))
            .ReturnsAsync(plantio);

        // Act
        await _service.RemoverAsync(1, 3);

        // Assert
        _repositoryMock.Verify(r => r.RemoverAsync(plantio, It.IsAny<CancellationToken>()), Times.Once);
    }
}
