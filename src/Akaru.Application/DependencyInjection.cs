using Akaru.Application.Services;
using Microsoft.Extensions.DependencyInjection;

namespace Akaru.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddScoped<UsuarioService>();
        services.AddScoped<PlantioService>();
        services.AddScoped<HistoricoService>();
        return services;
    }
}
