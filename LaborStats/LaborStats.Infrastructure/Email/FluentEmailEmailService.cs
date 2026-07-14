using FluentEmail.Core;
using LaborStats.Application.Abstractions;

namespace LaborStats.Infrastructure.Email;

internal sealed class FluentEmailEmailService(IFluentEmailFactory fluentEmailFactory) : IEmailService
{
    public async Task<bool> SendAsync(EmailMessage message, CancellationToken cancellationToken = default)
    {
        var response = await fluentEmailFactory
            .Create()
            .To(message.To)
            .Subject(message.Subject)
            .Body(message.Body, message.IsHtml)
            .SendAsync(cancellationToken);

        return response.Successful;
    }
}