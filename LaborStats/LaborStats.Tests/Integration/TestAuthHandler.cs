using System.Security.Claims;
using System.Text.Encodings.Web;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace LaborStats.Tests.Integration;

public sealed class TestAuthHandler(
    IOptionsMonitor<AuthenticationSchemeOptions> options,
    ILoggerFactory loggerFactory,
    UrlEncoder encoder)
    : AuthenticationHandler<AuthenticationSchemeOptions>(
        options,
        loggerFactory,
        encoder)
{
    public const string AuthenticationScheme = "TestScheme";
    public const string UserIdHeader = "Test-User-Id";
    public const string UserNameHeader = "Test-User-Name";
    public const string RoleHeader = "Test-Role";
    public const string UnauthenticatedHeader = "Test-Unauthenticated";
    public const string DefaultUserId = "f81ea23c-4962-4044-80c3-cc3bb4586228";

    protected override Task<AuthenticateResult> HandleAuthenticateAsync()
    {
        string? unauthenticated =
            Request.Headers[UnauthenticatedHeader]
                .FirstOrDefault();

        if (string.Equals(
                unauthenticated,
                "true",
                StringComparison.OrdinalIgnoreCase))
        {
            return Task.FromResult(
                AuthenticateResult.NoResult());
        }

        string userId =
            Request.Headers[UserIdHeader].FirstOrDefault()
            ?? DefaultUserId;

        string userName =
            Request.Headers[UserNameHeader].FirstOrDefault()
            ?? "integration-test-admin";

        string role =
            Request.Headers[RoleHeader].FirstOrDefault()
            ?? "Admin";

        Claim[] claims =
        [
            new Claim(ClaimTypes.NameIdentifier, userId),
            new Claim(ClaimTypes.Name, userName),
            new Claim(ClaimTypes.Role, role)
        ];

        ClaimsIdentity identity = new(
            claims,
            AuthenticationScheme);

        ClaimsPrincipal principal = new(identity);

        AuthenticationTicket ticket = new(
            principal,
            AuthenticationScheme);

        return Task.FromResult(
            AuthenticateResult.Success(ticket));
    }
}
