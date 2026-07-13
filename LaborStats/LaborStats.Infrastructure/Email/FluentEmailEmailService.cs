using System.Net;
using System.Net.Mail;
using FluentEmail.Core;
using LaborStats.Application.Abstractions;
using LaborStats.Application.Options;

namespace LaborStats.Infrastructure.Email;

public sealed class FluentEmailEmailService : IEmailService
{
    private readonly IFluentEmail _fluentEmail;

    public FluentEmailEmailService(IFluentEmail fluentEmail)
    {
        _fluentEmail = fluentEmail;
    }

    public async Task<bool> SendAsync(EmailMessage message, CancellationToken cancellationToken = default)
    {
        var response = await _fluentEmail
            .To(message.To)
            .Subject(message.Subject)
            .Body(message.Body, message.IsHtml)
            .SendAsync(cancellationToken);

        return response.Successful;
    }

    public static Func<SmtpClient> CreateSmtpClientFactory(EmailOptions options)
    {
        return () =>
        {
            var client = new SmtpClient(options.SmtpHost, options.SmtpPort)
            {
                EnableSsl = options.EnableSsl,
                DeliveryMethod = SmtpDeliveryMethod.Network,
                UseDefaultCredentials = string.IsNullOrEmpty(options.SmtpUsername),
            };

            if (!string.IsNullOrEmpty(options.SmtpUsername))
            {
                client.Credentials = new NetworkCredential(options.SmtpUsername, options.SmtpPassword);
            }

            return client;
        };
    }
}