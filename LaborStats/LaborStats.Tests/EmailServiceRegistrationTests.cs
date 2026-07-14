using System.Net;
using FluentEmail.Core.Interfaces;
using LaborStats.Application.Abstractions;
using LaborStats.Application.Options;
using LaborStats.Infrastructure;
using LaborStats.Infrastructure.Email;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace LaborStats.Tests;

public class EmailServiceRegistrationTests
{
    private static IConfiguration BuildConfiguration(IDictionary<string, string?> values)
    {
        return new ConfigurationBuilder()
            .AddInMemoryCollection(values)
            .Build();
    }

    private static readonly IDictionary<string, string?> ValidEmailConfig =
        new Dictionary<string, string?>
        {
            ["Email:FromEmail"] = "noreply@laborstats.local",
            ["Email:FromName"] = "Labor Stats",
            ["Email:SmtpHost"] = "smtp.example.com",
            ["Email:SmtpPort"] = "587",
            ["Email:SmtpUsername"] = "user",
            ["Email:SmtpPassword"] = "pwd",
            ["Email:EnableSsl"] = "true",
        };

    [Fact]
    public void AddEmailServices_RegistersIEmailService()
    {
        var services = new ServiceCollection();
        services.AddEmailServices(BuildConfiguration(ValidEmailConfig));

        using var provider = services.BuildServiceProvider();
        var emailService = provider.GetService<IEmailService>();

        Assert.NotNull(emailService);
        Assert.IsType<FluentEmailEmailService>(emailService);
    }

    [Fact]
    public void AddEmailServices_RegistersScopedSmtpSenderFromFactory()
    {
        var services = new ServiceCollection();
        services.AddEmailServices(BuildConfiguration(ValidEmailConfig));

        using var provider = services.BuildServiceProvider();
        using var scope1 = provider.CreateScope();
        using var scope2 = provider.CreateScope();
        var sender1 = scope1.ServiceProvider.GetRequiredService<ISender>();
        var sender2 = scope2.ServiceProvider.GetRequiredService<ISender>();

        Assert.NotSame(sender1, sender2);
    }

    [Fact]
    public void AddEmailServices_BindsOptionsFromConfiguration()
    {
        var services = new ServiceCollection();
        services.AddEmailServices(BuildConfiguration(ValidEmailConfig));

        using var provider = services.BuildServiceProvider();
        var options = provider.GetRequiredService<IOptions<EmailOptions>>().Value;

        Assert.Equal("noreply@laborstats.local", options.FromEmail);
        Assert.Equal("smtp.example.com", options.SmtpHost);
        Assert.Equal(587, options.SmtpPort);
        Assert.True(options.EnableSsl);
    }

    [Fact]
    public void AddEmailServices_SurfacesInvalidConfigViaOptionsValidation()
    {
        var invalidConfig = new Dictionary<string, string?>
        {
            ["Email:SmtpHost"] = "smtp.example.com",
            ["Email:SmtpPort"] = "25",
        };

        var services = new ServiceCollection();
        services.AddEmailServices(BuildConfiguration(invalidConfig));

        using var provider = services.BuildServiceProvider();
        var options = provider.GetRequiredService<IOptions<EmailOptions>>();

        Assert.Throws<OptionsValidationException>(() => options.Value);
    }

    [Fact]
    public void CreateSmtpClientFactory_AppliesCredentialsAndSsl()
    {
        var options = new EmailOptions
        {
            SmtpHost = "smtp.example.com",
            SmtpPort = 587,
            SmtpUsername = "user",
            SmtpPassword = "pwd",
            EnableSsl = true,
        };

        var factory = SmtpClientFactory.Create(options);
        using var client = factory();

        Assert.Equal("smtp.example.com", client.Host);
        Assert.Equal(587, client.Port);
        Assert.True(client.EnableSsl);
        Assert.False(client.UseDefaultCredentials);
        var credentials = Assert.IsType<NetworkCredential>(client.Credentials);
        Assert.Equal("user", credentials.UserName);
        Assert.Equal("pwd", credentials.Password);
    }

    [Fact]
    public void CreateSmtpClientFactory_UsesDefaultCredentialsWhenUsernameEmpty()
    {
        var options = new EmailOptions
        {
            SmtpHost = "smtp.example.com",
            SmtpPort = 25,
            EnableSsl = false,
        };

        var factory = SmtpClientFactory.Create(options);
        using var client = factory();

        Assert.True(client.UseDefaultCredentials);
    }

    [Fact]
    public void CreateSmtpClientFactory_CreatesNewInstancePerInvocation()
    {
        var options = new EmailOptions
        {
            SmtpHost = "smtp.example.com",
            SmtpPort = 25,
        };

        var factory = SmtpClientFactory.Create(options);

        using var first = factory();
        using var second = factory();

        Assert.NotSame(first, second);
    }
}