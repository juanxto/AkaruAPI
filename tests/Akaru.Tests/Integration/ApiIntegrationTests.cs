using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using Akaru.Application.DTOs;
using Akaru.Infrastructure.Persistence;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace Akaru.Tests.Integration;

public class ApiIntegrationTests : IClassFixture<AkaruApiFactory>
{
    private readonly AkaruApiFactory _factory;

    public ApiIntegrationTests(AkaruApiFactory factory)
    {
        _factory = factory;
    }

    [Fact]
    public async Task Health_DeveRetornarHealthy()
    {
        var client = _factory.CreateClient();
        var response = await client.GetAsync("/health");

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }

    [Fact]
    public async Task FluxoMobile_Completo_DeveFuncionar()
    {
        await ResetarBancoAsync();
        var client = _factory.CreateClient();
        var token = await RegistrarEObterTokenAsync(client);

        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

        var perfil = await client.GetAsync("/api/usuarios/me");
        Assert.Equal(HttpStatusCode.OK, perfil.StatusCode);

        var atualizar = await client.PutAsJsonAsync("/api/usuarios/me", new AtualizarUsuarioDto(
            "Juan Pablo", -23.5505m, -46.6333m, "São Paulo", "SP"));
        Assert.Equal(HttpStatusCode.OK, atualizar.StatusCode);

        var plantioDto = new CriarPlantioDto(
            3, -23.5505m, -46.6333m, DateTime.UtcNow, "Solo argiloso", "São Paulo", "SP", null);

        var criarPlantio = await client.PostAsJsonAsync("/api/plantios", plantioDto);
        Assert.Equal(HttpStatusCode.Created, criarPlantio.StatusCode);

        var listarPlantios = await client.GetAsync("/api/plantios");
        Assert.Equal(HttpStatusCode.OK, listarPlantios.StatusCode);
        var plantios = await listarPlantios.Content.ReadFromJsonAsync<List<PlantioResponseDto>>();
        Assert.NotNull(plantios);
        Assert.Single(plantios);

        var historicoDto = new SalvarHistoricoDto(
            3, "Milho", "Plante entre setembro e novembro.", 87.5m,
            -23.5505m, -46.6333m, "Área irrigada", "{\"temperatura\":28}", null);

        var salvarHistorico = await client.PostAsJsonAsync("/api/historico", historicoDto);
        Assert.Equal(HttpStatusCode.Created, salvarHistorico.StatusCode);

        var listarHistorico = await client.GetAsync("/api/historico");
        Assert.Equal(HttpStatusCode.OK, listarHistorico.StatusCode);
        var historicos = await listarHistorico.Content.ReadFromJsonAsync<List<HistoricoResponseDto>>();
        Assert.NotNull(historicos);
        Assert.Single(historicos);
    }

    [Fact]
    public async Task Login_ComCredenciaisInvalidas_DeveRetornar403()
    {
        var client = _factory.CreateClient();
        var response = await client.PostAsJsonAsync("/api/auth/login",
            new LoginDto("inexistente@email.com", "senhaerrada"));

        Assert.Equal(HttpStatusCode.Forbidden, response.StatusCode);
    }

    [Fact]
    public async Task RequisicaoSemToken_DeveRetornar401()
    {
        var client = _factory.CreateClient();
        var response = await client.GetAsync("/api/usuarios/me");

        Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
    }

    private static async Task<string> RegistrarEObterTokenAsync(HttpClient client)
    {
        var email = $"teste-{Guid.NewGuid():N}@akaru.local";
        var response = await client.PostAsJsonAsync("/api/auth/register",
            new RegisterDto("Teste Integracao", email, "senha123"));

        Assert.Equal(HttpStatusCode.Created, response.StatusCode);
        var auth = await response.Content.ReadFromJsonAsync<AuthResponseDto>();
        Assert.NotNull(auth);
        return auth.Token;
    }

    private async Task ResetarBancoAsync()
    {
        await using var scope = _factory.Services.CreateAsyncScope();
        var db = scope.ServiceProvider.GetRequiredService<AkaruDbContext>();
        await db.Database.EnsureDeletedAsync();
        await db.Database.EnsureCreatedAsync();
    }
}
