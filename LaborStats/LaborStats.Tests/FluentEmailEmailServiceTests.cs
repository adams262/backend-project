using FluentEmail.Core;
using FluentEmail.Core.Interfaces;
using FluentEmail.Core.Models;
using LaborStats.Application.Abstractions;
using LaborStats.Infrastructure.Email;
using Microsoft.Extensions.DependencyInjection;

namespace LaborStats.Tests;

public class FluentEmailEmailServiceTests
{
    [Fact]
    public async Task SendAsync_UsesFactoryAndSendsEmail()
    {
        var services = new ServiceCollection();
        services.AddFluentEmail("from@test.com", "Test Sender");
        services.AddSingleton<ISender, FakeSender>();
        services.AddTransient<IEmailService, FluentEmailEmailService>();

        using var provider = services.BuildServiceProvider();
        var sender = (FakeSender)provider.GetRequiredService<ISender>();
        var emailService = provider.GetRequiredService<IEmailService>();

        var message = new EmailMessage(
            To: "to@test.com",
            Subject: "Hello",
            Body: "<p>World</p>",
            IsHtml: true);

        var result = await emailService.SendAsync(message);

        Assert.True(result);
        Assert.NotNull(sender.LastEmail);
        Assert.Equal("to@test.com", sender.LastEmail.Data.ToAddresses.Single().EmailAddress);
        Assert.Equal("Hello", sender.LastEmail.Data.Subject);
        Assert.Equal("<p>World</p>", sender.LastEmail.Data.Body);
        Assert.True(sender.LastEmail.Data.IsHtml);
    }

    private sealed class FakeSender : ISender
    {
        public IFluentEmail? LastEmail { get; private set; }

        public SendResponse Send(IFluentEmail email, CancellationToken? token = null)
        {
            LastEmail = email;
            return new SendResponse();
        }

        public Task<SendResponse> SendAsync(IFluentEmail email, CancellationToken? token = null)
        {
            LastEmail = email;
            return Task.FromResult(new SendResponse());
        }
    }
}
