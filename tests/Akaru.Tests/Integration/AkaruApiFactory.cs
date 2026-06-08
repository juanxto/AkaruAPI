using Akaru.Infrastructure.Persistence;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace Akaru.Tests.Integration;

public class AkaruApiFactory : WebApplicationFactory<Program>
{
    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.UseEnvironment("Development");

        builder.ConfigureTestServices(services =>
        {
            services.RemoveAll(typeof(DbContextOptions<AkaruDbContext>));
            services.RemoveAll(typeof(AkaruDbContext));

            services.AddDbContext<AkaruDbContext>(options =>
                options.UseInMemoryDatabase("AkaruIntegrationTests"));
        });
    }
}
