using Testcontainers.PostgreSql;

namespace LaborStats.Tests.Integration;

public sealed class TestDatabase : IAsyncDisposable
{
    public static TestDatabase Instance { get; } = new();

    public PostgreSqlContainer? Container { get; private set; }

    public string ConnectionString =>
        Container?.GetConnectionString()
        ?? throw new InvalidOperationException(
            "Test database container has not been initialized.");

    private TestDatabase()
    {
    }

    public async ValueTask InitializeAsync()
    {
        if (Container is not null)
        {
            return;
        }

        PostgreSqlContainer container = new PostgreSqlBuilder("postgres:18")
            .WithDatabase("labor_stats_integration_tests")
            .WithUsername("postgres")
            .WithPassword("postgres")
            .WithCleanUp(true)
            .Build();

        await container.StartAsync();

        Container = container;
    }

    public async ValueTask DisposeAsync()
    {
        PostgreSqlContainer? container = Container;
        Container = null;

        if (container is not null)
        {
            await container.DisposeAsync();
        }
    }
}
