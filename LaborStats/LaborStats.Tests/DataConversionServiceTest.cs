using LaborStats.Application.Abstractions;
using LaborStats.Infrastructure;
using LaborStats.Infrastructure.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;


namespace LaborStats.Tests;

public class DataConversionServiceRegistrationTests
{
    private static IConfiguration BuildConfiguration(IDictionary<string, string?> values)
    {
        return new ConfigurationBuilder()
            .AddInMemoryCollection(values)
            .Build();
    }

    private static readonly IDictionary<string, string?> MinimalConfig =
        new Dictionary<string, string?>
        {
            ["Jwt:AccessTokenExpirationMinutes"] = "60",
            ["AdminUser:Email"] = "admin@laborstats.local",
            ["AdminUser:Password"] = "Admin123!"
        };

    private const string DummyConnectionString = "Host=localhost;Database=test;Username=postgres;Password=postgres";

    [Fact]
    public void AddInfrastructure_RegistersIDataConversionService()
    {
        var services = new ServiceCollection();
        services.AddInfrastructure(DummyConnectionString, BuildConfiguration(MinimalConfig));

        using var provider = services.BuildServiceProvider();
        var service = provider.GetService<IDataConversionService>();

        Assert.NotNull(service);
        Assert.IsType<DataConversionService>(service);
    }

    [Fact]
    public void AddInfrastructure_RegistersDataConversionServiceAsScoped()
    {
        var services = new ServiceCollection();
        services.AddInfrastructure(DummyConnectionString, BuildConfiguration(MinimalConfig));

        using var provider = services.BuildServiceProvider();
        
        using var scope1 = provider.CreateScope();
        using var scope2 = provider.CreateScope();

        var instance1Scope1 = scope1.ServiceProvider.GetRequiredService<IDataConversionService>();
        var instance2Scope1 = scope1.ServiceProvider.GetRequiredService<IDataConversionService>();
        var instance1Scope2 = scope2.ServiceProvider.GetRequiredService<IDataConversionService>();

        Assert.Same(instance1Scope1, instance2Scope1);

        Assert.NotSame(instance1Scope1, instance1Scope2);
    }
}