namespace LaborStats.Tests.Integration;

public static class TestCollections
{
    public const string Integration = "Integration tests";
}

[CollectionDefinition(
    TestCollections.Integration,
    DisableParallelization = true)]
public sealed class IntegrationTestCollection : ICollectionFixture<TestSetup>
{
}
