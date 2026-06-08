using Akaru.Application.DTOs;
using Akaru.Application.Interfaces;
using Akaru.Application.Services;
using Akaru.Domain.Entities;
using Akaru.Domain.Exceptions;
using Moq;
using Xunit;

namespace Akaru.Tests.Services;

public class UsuarioServiceTests
{
    private readonly Mock<IUsuarioRepository> _repositoryMock;
    private readonly UsuarioService _service;

    public UsuarioServiceTests()
    {
        _repositoryMock = new Mock<IUsuarioRepository>();
        _service = new UsuarioService(_repositoryMock.Object);
    }

    [Fact]
    public async Task SincronizarAsync_DeveRetornarUsuarioExistente_QuandoJaCadastrado()
    {
        var existente = new Usuario
        {
            Id = 1,
            FirebaseUid = "uid-123",
            Nome = "Juan",
            Email = "juan@email.com",
            DataCadastro = DateTime.UtcNow
        };

        _repositoryMock
            .Setup(r => r.ObterPorFirebaseUidAsync("uid-123", It.IsAny<CancellationToken>()))
            .ReturnsAsync(existente);

        var resultado = await _service.SincronizarAsync(
            new FirebaseUserInfo("uid-123", "juan@email.com", "Juan"));

        Assert.Equal(1, resultado.Id);
        Assert.Equal("Juan", resultado.Nome);
        _repositoryMock.Verify(r => r.CriarAsync(It.IsAny<Usuario>(), It.IsAny<CancellationToken>()), Times.Never);
    }

    [Fact]
    public async Task SincronizarAsync_DeveCriarUsuario_QuandoNaoExiste()
    {
        _repositoryMock
            .Setup(r => r.ObterPorFirebaseUidAsync("uid-novo", It.IsAny<CancellationToken>()))
            .ReturnsAsync((Usuario?)null);

        _repositoryMock
            .Setup(r => r.CriarAsync(It.IsAny<Usuario>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync((Usuario u, CancellationToken _) =>
            {
                u.Id = 5;
                return u;
            });

        var resultado = await _service.SincronizarAsync(
            new FirebaseUserInfo("uid-novo", "novo@email.com", "Novo Usuario"));

        Assert.Equal(5, resultado.Id);
        Assert.Equal("novo@email.com", resultado.Email);
        _repositoryMock.Verify(r => r.CriarAsync(It.IsAny<Usuario>(), It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task ObterIdPorFirebaseUidAsync_DeveLancarNotFound_QuandoUsuarioNaoSincronizado()
    {
        _repositoryMock
            .Setup(r => r.ObterPorFirebaseUidAsync("uid-ausente", It.IsAny<CancellationToken>()))
            .ReturnsAsync((Usuario?)null);

        await Assert.ThrowsAsync<NotFoundException>(() =>
            _service.ObterIdPorFirebaseUidAsync("uid-ausente"));
    }

    [Fact]
    public async Task AtualizarPerfilAsync_DeveAtualizarCamposInformados()
    {
        var usuario = new Usuario
        {
            Id = 1,
            FirebaseUid = "uid-1",
            Nome = "Antigo",
            Email = "a@b.com",
            DataCadastro = DateTime.UtcNow
        };

        _repositoryMock
            .Setup(r => r.ObterPorIdAsync(1, It.IsAny<CancellationToken>()))
            .ReturnsAsync(usuario);

        var dto = new AtualizarUsuarioDto("Juan Pablo", -23.5505m, -46.6333m, "São Paulo", "SP");
        var resultado = await _service.AtualizarPerfilAsync(1, dto);

        Assert.Equal("Juan Pablo", resultado.Nome);
        Assert.Equal("São Paulo", resultado.Cidade);
        Assert.Equal("SP", resultado.Estado);
        _repositoryMock.Verify(r => r.AtualizarAsync(usuario, It.IsAny<CancellationToken>()), Times.Once);
    }
}
