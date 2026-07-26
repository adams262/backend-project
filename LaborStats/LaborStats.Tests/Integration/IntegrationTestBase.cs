namespace LaborStats.Tests.Integration;

public abstract class IntegrationTestBase(
    TestSetup fixture) : IAsyncLifetime
{
    protected TestSetup Fixture { get; } = fixture;

    public Task InitializeAsync()
    {
        return Task.CompletedTask;
    }

    public Task DisposeAsync()
    {
        return Fixture.ResetDatabaseAsync();
    }
}
