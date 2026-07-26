using System.Data.Common;
using LaborStats.Infrastructure.Data;
using LaborStats.Infrastructure.Data.Seed;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Respawn;
using Respawn.Graph;

namespace LaborStats.Tests.Integration;

public sealed class TestSetup : IAsyncLifetime
{
    private readonly Dictionary<string, string?>
        _previousEnvironmentVariables = [];

    private CustomWebAppFactory<Program>? _webApiFactory;
    private HttpClient? _webApiClient;
    private Respawner? _respawner;

    public CustomWebAppFactory<Program> WebApiFactory =>
        _webApiFactory
        ?? throw new InvalidOperationException(
            "Web API factory has not been initialized.");

    public HttpClient WebApiClient =>
        _webApiClient
        ?? throw new InvalidOperationException(
            "Web API client has not been initialized.");

    public async Task InitializeAsync()
    {
        await TestDatabase.Instance.InitializeAsync();

        string connectionString =
            TestDatabase.Instance.ConnectionString;

        await ApplyMigrationsAsync(connectionString);


        SetTestEnvironmentVariables(connectionString);

        try
        {
            _webApiFactory =
                new CustomWebAppFactory<Program>(
                    connectionString);


            _webApiClient =
                _webApiFactory.CreateClient();
        }
        finally
        {
            RestoreEnvironmentVariables();
        }

        await ConfigureRespawnerAsync();
    }

    private static async Task ApplyMigrationsAsync(
        string connectionString)
    {
        string migrationsAssembly =
            typeof(LaborStatsDbContext)
                .Assembly
                .GetName()
                .Name
            ?? throw new InvalidOperationException(
                "Could not determine the migrations assembly name.");

        DbContextOptions<LaborStatsDbContext> options =
            new DbContextOptionsBuilder<LaborStatsDbContext>()
                .UseNpgsql(
                    connectionString,
                    npgsqlOptions =>
                    {
                        npgsqlOptions.MigrationsAssembly(
                            migrationsAssembly);
                    })
                .UseSnakeCaseNamingConvention()
                .Options;

        await using LaborStatsDbContext dbContext =
            new(options);

        await dbContext.Database.MigrateAsync();
    }

    private async Task ConfigureRespawnerAsync()
    {
        await using AsyncServiceScope scope =
            WebApiFactory.Services.CreateAsyncScope();

        LaborStatsDbContext dbContext =
            scope.ServiceProvider
                .GetRequiredService<LaborStatsDbContext>();

        DbConnection connection =
            dbContext.Database.GetDbConnection();

        await connection.OpenAsync();

        try
        {
            _respawner = await Respawner.CreateAsync(
                connection,
                new RespawnerOptions
                {
                    DbAdapter = DbAdapter.Postgres,


                    SchemasToInclude =
                    [
                        "usr",
                        "import"
                    ],


                    TablesToIgnore =
                    [
                        new Table("usr", "roles")
                    ]
                });
        }
        finally
        {
            await connection.CloseAsync();
        }
    }

    public async Task ResetDatabaseAsync()
    {
        Respawner respawner =
            _respawner
            ?? throw new InvalidOperationException(
                "Respawner has not been initialized.");

        await using AsyncServiceScope scope =
            WebApiFactory.Services.CreateAsyncScope();

        LaborStatsDbContext dbContext =
            scope.ServiceProvider
                .GetRequiredService<LaborStatsDbContext>();

        DbConnection connection =
            dbContext.Database.GetDbConnection();

        await connection.OpenAsync();

        try
        {
            await respawner.ResetAsync(connection);
        }
        finally
        {
            await connection.CloseAsync();
        }


        await dbContext.Role
            .Where(role =>
                role.Name != "Admin"
                && role.Name != "BaseUser")
            .ExecuteDeleteAsync();

        AdminSeeder adminSeeder =
            scope.ServiceProvider
                .GetRequiredService<AdminSeeder>();

        await adminSeeder.SeedAsync(CancellationToken.None);
    }

    private void SetTestEnvironmentVariables(
        string connectionString)
    {
        SetEnvironmentVariable(
            "ConnectionStrings__DefaultConnection",
            connectionString);
    }

    private void SetEnvironmentVariable(
        string name,
        string value)
    {
        _previousEnvironmentVariables[name] =
            Environment.GetEnvironmentVariable(name);

        Environment.SetEnvironmentVariable(
            name,
            value);
    }

    private void RestoreEnvironmentVariables()
    {
        foreach (
            KeyValuePair<string, string?> environmentVariable
            in _previousEnvironmentVariables)
        {
            Environment.SetEnvironmentVariable(
                environmentVariable.Key,
                environmentVariable.Value);
        }

        _previousEnvironmentVariables.Clear();
    }

    public async Task DisposeAsync()
    {
        _webApiClient?.Dispose();
        _webApiFactory?.Dispose();

        RestoreEnvironmentVariables();

        await TestDatabase.Instance.DisposeAsync();
    }
}
