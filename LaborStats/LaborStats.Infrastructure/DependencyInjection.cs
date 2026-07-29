using LaborStats.Application.Abstractions;
using LaborStats.Application.Options;
using LaborStats.Infrastructure.Data;
using LaborStats.Infrastructure.Data.Seed;
using LaborStats.Infrastructure.Email;
using LaborStats.Infrastructure.Options;
using LaborStats.Infrastructure.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using LaborStats.Domain.Entities;
using Microsoft.AspNetCore.Identity;

namespace LaborStats.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, string connectionString, IConfiguration configuration)
    {
        services.AddDbContext<LaborStatsDbContext>(options =>
        {
            options.UseNpgsql(connectionString);
            options.UseSnakeCaseNamingConvention();
        });

        services.AddOptions<AdminUserOptions>()
            .Bind(configuration.GetSection(AdminUserOptions.SectionName));

        services.AddOptions<JwtOptions>()
            .Bind(configuration.GetSection(JwtOptions.SectionName))
            .Validate(o => o.AccessTokenExpirationMinutes > 0, "Jwt:AccessTokenExpirationMinutes must be greater than zero.")
            .ValidateOnStart();

        services.AddSingleton(TimeProvider.System);

        services.AddScoped<IPasswordHasher<Users>, PasswordHasher<Users>>();

        services.AddScoped<IRoleService, RoleService>();
        services.AddScoped<IUserService, UserService>();
        services.AddScoped<IAuthService, AuthService>();

        services.AddScoped<IImportQueryService,ImportQueryService>();
        services.AddScoped<IImportValidator, ImportValidator>();

        return services.AddScoped<AdminSeeder>();
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
