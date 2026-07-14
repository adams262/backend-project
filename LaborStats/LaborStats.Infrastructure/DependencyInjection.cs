using LaborStats.Application.Abstractions;
using LaborStats.Application.Options;
using LaborStats.Infrastructure.Data;
using LaborStats.Infrastructure.Email;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace LaborStats.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, string connectionString)
    {
        services.AddDbContext<LaborStatsDbContext>(options =>
        {
            options.UseNpgsql(connectionString);
            options.UseSnakeCaseNamingConvention();
        });

        return services;
    }

    public static IServiceCollection AddEmailServices(this IServiceCollection services, IConfiguration configuration)
    {
        var section = configuration.GetSection(EmailOptions.SectionName);

        services.AddOptions<EmailOptions>()
            .Bind(section)
            .Validate(o =>
                !string.IsNullOrWhiteSpace(o.FromEmail) &&
                !string.IsNullOrWhiteSpace(o.SmtpHost) &&
                HasConsistentCredentials(o),
                "Email:FromEmail and Email:SmtpHost are required configuration values, and SMTP credentials (SmtpUsername and SmtpPassword) must be both set or both empty.")
            .ValidateOnStart();

        var options = section.Get<EmailOptions>() ?? new EmailOptions();

        services
            .AddFluentEmail(options.FromEmail, options.FromName)
            .AddSmtpSender(SmtpClientFactory.Create(options));

        return services.AddTransient<IEmailService, FluentEmailEmailService>();
    }

    private static bool HasConsistentCredentials(EmailOptions options) =>
        SmtpClientFactory.HasValidCredentials(options) ||
        (string.IsNullOrWhiteSpace(options.SmtpUsername) &&
            string.IsNullOrWhiteSpace(options.SmtpPassword));
}