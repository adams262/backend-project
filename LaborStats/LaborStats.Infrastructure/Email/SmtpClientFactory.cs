using System.Net;
using System.Net.Mail;
using LaborStats.Application.Options;

namespace LaborStats.Infrastructure.Email;

internal static class SmtpClientFactory
{
    public static Func<SmtpClient> Create(EmailOptions options)
    {
        ArgumentNullException.ThrowIfNull(options);

        return () =>
        {
            var hasCredentials = HasValidCredentials(options);

            var client = new SmtpClient(
                options.SmtpHost,
                options.SmtpPort)
            {
                EnableSsl = options.EnableSsl,
                DeliveryMethod = SmtpDeliveryMethod.Network,
                UseDefaultCredentials = !hasCredentials,
            };

            if (hasCredentials)
            {
                client.Credentials = new NetworkCredential(
                    options.SmtpUsername,
                    options.SmtpPassword);
            }

            return client;
        };
    }

    internal static bool HasValidCredentials(EmailOptions options) =>
        !string.IsNullOrWhiteSpace(options.SmtpUsername) &&
        !string.IsNullOrWhiteSpace(options.SmtpPassword);
}
