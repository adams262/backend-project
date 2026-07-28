namespace LaborStats.Application.Abstractions;

public interface IEmailService
{
    Task<bool> SendAsync(EmailMessage message, CancellationToken cancellationToken = default);
}