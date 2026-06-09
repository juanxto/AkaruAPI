using Akaru.Application.DTOs;
using Akaru.Application.Interfaces;
using Akaru.Application.Services;
using Akaru.Domain.Entities;
using Akaru.Domain.Exceptions;
using Moq;
using Xunit;

namespace Akaru.Tests.Services;

public class AuthServiceTests
{
    private readonly Mock<IUsuarioRepository> _repositoryMock;
    private readonly Mock<IJwtTokenGenerator> _jwtMock;
    private readonly AuthService _service;

    public AuthServiceTests()
    {
        _repositoryMock = new Mock<IUsuarioRepository>();
        _jwtMock = new Mock<IJwtTokenGenerator>();
        _jwtMock.Setup(j => j.GerarToken(It.IsAny<Usuario>()))
            .Returns(("token-jwt-teste", DateTime.UtcNow.AddHours(24)));
        _service = new AuthService(_repositoryMock.Object, _jwtMock.Object);
    }

    [Fact]
    public async Task LoginAsync_DeveRetornarToken_QuandoCredenciaisValidas()
    {
        var usuario = new Usuario
        {
            Id = 1,
            Email = "joao@email.com",
            Nome = "Joao",
            Senha = PasswordHasher.Hash("senha123")
        };

        _repositoryMock.Setup(r => r.ObterPorEmailAsync("joao@email.com", It.IsAny<CancellationToken>()))
            .ReturnsAsync(usuario);

        var resultado = await _service.LoginAsync(new LoginDto("joao@email.com", "senha123"));

        Assert.Equal("token-jwt-teste", resultado.Token);
        Assert.Equal("joao@email.com", resultado.Usuario.Email);
    }

    [Fact]
    public async Task LoginAsync_DeveLancarAcessoNegado_QuandoSenhaInvalida()
    {
        _repositoryMock.Setup(r => r.ObterPorEmailAsync("joao@email.com", It.IsAny<CancellationToken>()))
            .ReturnsAsync(new Usuario { Email = "joao@email.com", Senha = PasswordHasher.Hash("senha123") });

        await Assert.ThrowsAsync<AcessoNegadoException>(() =>
            _service.LoginAsync(new LoginDto("joao@email.com", "errada")));
    }

    [Fact]
    public async Task RegistrarAsync_DeveCriarUsuario_QuandoEmailNovo()
    {
        _repositoryMock.Setup(r => r.ObterPorEmailAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync((Usuario?)null);
        _repositoryMock.Setup(r => r.CriarAsync(It.IsAny<Usuario>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync((Usuario u, CancellationToken _) =>
            {
                u.Id = 5;
                return u;
            });

        var resultado = await _service.RegistrarAsync(
            new RegisterDto("Maria", "maria@email.com", "senha123"));

        Assert.Equal(5, resultado.Usuario.Id);
        Assert.Equal("token-jwt-teste", resultado.Token);
        _repositoryMock.Verify(r => r.CriarAsync(It.IsAny<Usuario>(), It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task RegistrarAsync_DeveLancarDomainException_QuandoEmailJaExiste()
    {
        _repositoryMock.Setup(r => r.ObterPorEmailAsync("maria@email.com", It.IsAny<CancellationToken>()))
            .ReturnsAsync(new Usuario { Email = "maria@email.com" });

        await Assert.ThrowsAsync<DomainException>(() =>
            _service.RegistrarAsync(new RegisterDto("Maria", "maria@email.com", "senha123")));
    }
}
