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
    private const string DevToken = "dev-test-token";
    private readonly HttpClient _client;
    private readonly AkaruApiFactory _factory;

    public ApiIntegrationTests(AkaruApiFactory factory)
    {
        _factory = factory;
        _client = factory.CreateClient();
        _client.DefaultRequestHeaders.Authorization =
            new AuthenticationHeaderValue("Bearer", DevToken);
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

        var sync = await _client.PostAsync("/api/usuarios/sync", null);
        Assert.Equal(HttpStatusCode.OK, sync.StatusCode);
        var usuario = await sync.Content.ReadFromJsonAsync<UsuarioResponseDto>();
        Assert.NotNull(usuario);
        Assert.True(usuario.Id > 0);

        var perfil = await _client.GetAsync("/api/usuarios/me");
        Assert.Equal(HttpStatusCode.OK, perfil.StatusCode);

        var atualizar = await _client.PutAsJsonAsync("/api/usuarios/me", new AtualizarUsuarioDto(
            "Juan Pablo", -23.5505m, -46.6333m, "São Paulo", "SP"));
        Assert.Equal(HttpStatusCode.OK, atualizar.StatusCode);

        var plantioDto = new CriarPlantioDto(
            3, -23.5505m, -46.6333m, DateTime.UtcNow, "Solo argiloso", "São Paulo", "SP", null);

        var criarPlantio = await _client.PostAsJsonAsync("/api/plantios", plantioDto);
        Assert.Equal(HttpStatusCode.Created, criarPlantio.StatusCode);

        var listarPlantios = await _client.GetAsync("/api/plantios");
        Assert.Equal(HttpStatusCode.OK, listarPlantios.StatusCode);
        var plantios = await listarPlantios.Content.ReadFromJsonAsync<List<PlantioResponseDto>>();
        Assert.NotNull(plantios);
        Assert.Single(plantios);

        var historicoDto = new SalvarHistoricoDto(
            3, "Milho", "Plante entre setembro e novembro.", 87.5m,
            -23.5505m, -46.6333m, "Área irrigada", "{\"temperatura\":28}", null);

        var salvarHistorico = await _client.PostAsJsonAsync("/api/historico", historicoDto);
        Assert.Equal(HttpStatusCode.Created, salvarHistorico.StatusCode);

        var listarHistorico = await _client.GetAsync("/api/historico");
        Assert.Equal(HttpStatusCode.OK, listarHistorico.StatusCode);
        var historicos = await listarHistorico.Content.ReadFromJsonAsync<List<HistoricoResponseDto>>();
        Assert.NotNull(historicos);
        Assert.Single(historicos);
    }

    [Fact]
    public async Task Plantios_SemSync_DeveRetornar404()
    {
        await ResetarBancoAsync();

        var client = _factory.CreateClient();
        client.DefaultRequestHeaders.Authorization =
            new AuthenticationHeaderValue("Bearer", "outro-usuario-sem-sync");

        var response = await client.GetAsync("/api/plantios");

        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        var body = await response.Content.ReadAsStringAsync();
        Assert.Contains("sync", body, StringComparison.OrdinalIgnoreCase);
    }

    [Fact]
    public async Task RequisicaoSemToken_DeveRetornar401()
    {
        var client = _factory.CreateClient();
        var response = await client.GetAsync("/api/usuarios/me");

        Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
    }

    private async Task ResetarBancoAsync()
    {
        await using var scope = _factory.Services.CreateAsyncScope();
        var db = scope.ServiceProvider.GetRequiredService<AkaruDbContext>();
        await db.Database.EnsureDeletedAsync();
        await db.Database.EnsureCreatedAsync();
    }
}
