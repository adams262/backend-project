using LaborStats.Infrastructure.Data;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace LaborStats.Tests.Integration;

public sealed class CustomWebAppFactory<TProgram>(
    string connectionString)
    : WebApplicationFactory<TProgram>
    where TProgram : class
{
    protected override void ConfigureWebHost(
        IWebHostBuilder builder)
    {
        builder.UseEnvironment("Testing");

        builder.ConfigureServices(services =>
        {
            ReplaceDatabase(services);
            ConfigureTestAuthentication(services);
        });
    }

    private void ReplaceDatabase(
    IServiceCollection services)
    {
        services.RemoveAll<
            IDbContextOptionsConfiguration<LaborStatsDbContext>>();

        services.RemoveAll<
            DbContextOptions<LaborStatsDbContext>>();

        services.RemoveAll<LaborStatsDbContext>();

        services.AddDbContext<LaborStatsDbContext>(options =>
        {
            options
                .UseNpgsql(
                    connectionString,
                    npgsqlOptions =>
                    {
                        npgsqlOptions.MigrationsAssembly(
                            typeof(LaborStatsDbContext)
                                .Assembly
                                .GetName()
                                .Name);
                    })
                .UseSnakeCaseNamingConvention();
        });
    }

    private static void ConfigureTestAuthentication(
        IServiceCollection services)
    {
        services
            .AddAuthentication(options =>
            {
                options.DefaultScheme =
                    TestAuthHandler.AuthenticationScheme;

                options.DefaultAuthenticateScheme =
                    TestAuthHandler.AuthenticationScheme;

                options.DefaultChallengeScheme =
                    TestAuthHandler.AuthenticationScheme;
            })
            .AddScheme<
                AuthenticationSchemeOptions,
                TestAuthHandler>(
                TestAuthHandler.AuthenticationScheme,
                _ =>
                {
                });
    }
}
