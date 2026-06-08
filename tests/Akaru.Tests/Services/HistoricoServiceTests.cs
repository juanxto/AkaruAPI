using Akaru.Application.DTOs;
using Akaru.Application.Interfaces;
using Akaru.Application.Services;
using Akaru.Domain.Entities;
using Akaru.Domain.Exceptions;
using Moq;
using Xunit;

namespace Akaru.Tests.Services;

public class HistoricoServiceTests
{
    private readonly Mock<IHistoricoRepository> _repositoryMock;
    private readonly HistoricoService _service;

    public HistoricoServiceTests()
    {
        _repositoryMock = new Mock<IHistoricoRepository>();
        _service = new HistoricoService(_repositoryMock.Object);
    }

    [Fact]
    public async Task SalvarAsync_DeveRetornarHistorico_QuandoTextoValido()
    {
        var dto = new SalvarHistoricoDto(
            CulturaId: 3,
            CulturaNome: "Milho",
            TextoRecomendacao: "Plante entre setembro e novembro.",
            Score: 87.5m,
            Latitude: -23.5505m,
            Longitude: -46.6333m,
            Detalhes: "Área irrigada",
            DadosClimaticos: "{\"temperatura\":28}",
            RecomendacaoCompleta: null);

        _repositoryMock
            .Setup(r => r.CriarAsync(It.IsAny<HistoricoRecomendacao>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync((HistoricoRecomendacao h, CancellationToken _) =>
            {
                h.Id = 7;
                return h;
            });

        var resultado = await _service.SalvarAsync(1, dto);

        Assert.Equal(7, resultado.Id);
        Assert.Equal("Milho", resultado.CulturaNome);
        Assert.Equal(87.5m, resultado.Score);
        _repositoryMock.Verify(r => r.CriarAsync(It.IsAny<HistoricoRecomendacao>(), It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task SalvarAsync_DeveLancarDomainException_QuandoTextoVazio()
    {
        var dto = new SalvarHistoricoDto(null, null, "   ", null, null, null, null, null, null);

        await Assert.ThrowsAsync<DomainException>(() => _service.SalvarAsync(1, dto));
        _repositoryMock.Verify(r => r.CriarAsync(It.IsAny<HistoricoRecomendacao>(), It.IsAny<CancellationToken>()), Times.Never);
    }

    [Fact]
    public async Task ObterPorIdAsync_DeveLancarAcessoNegado_QuandoHistoricoDeOutroUsuario()
    {
        _repositoryMock
            .Setup(r => r.ObterPorIdAsync(10, It.IsAny<CancellationToken>()))
            .ReturnsAsync(new HistoricoRecomendacao
            {
                Id = 10,
                UsuarioId = 99,
                TextoRecomendacao = "Texto",
                DataGeracao = DateTime.UtcNow
            });

        await Assert.ThrowsAsync<AcessoNegadoException>(() => _service.ObterPorIdAsync(1, 10));
    }

    [Fact]
    public async Task ListarPorUsuarioAsync_DeveRetornarResumoTruncado_ParaTextosLongos()
    {
        var textoLongo = new string('A', 150);
        _repositoryMock
            .Setup(r => r.ListarPorUsuarioAsync(1, It.IsAny<CancellationToken>()))
            .ReturnsAsync(new List<HistoricoRecomendacao>
            {
                new()
                {
                    Id = 1,
                    UsuarioId = 1,
                    TextoRecomendacao = textoLongo,
                    DataGeracao = DateTime.UtcNow
                }
            });

        var resultado = await _service.ListarPorUsuarioAsync(1);

        Assert.Single(resultado);
        Assert.NotNull(resultado[0].Resumo);
        Assert.True(resultado[0].Resumo!.Length <= 120);
        Assert.EndsWith("...", resultado[0].Resumo);
    }
}
