using Akaru.Application.Interfaces;
using Akaru.Infrastructure.Auth;
using Akaru.Infrastructure.Persistence;
using Akaru.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Akaru.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("Oracle")
            ?? throw new InvalidOperationException("Connection string 'Oracle' não configurada.");

        services.AddDbContext<AkaruDbContext>(options =>
            options.UseOracle(connectionString));

        services.AddScoped<IUsuarioRepository, UsuarioRepository>();
        services.AddScoped<IPlantioRepository, PlantioRepository>();
        services.AddScoped<IHistoricoRepository, HistoricoRepository>();
        services.AddSingleton<IFirebaseAuthService, FirebaseAuthService>();

        return services;
    }
}
