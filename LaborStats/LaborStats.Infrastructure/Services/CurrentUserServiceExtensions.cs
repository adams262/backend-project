using LaborStats.Application.Abstractions;
using LaborStats.Domain.Exceptions;

namespace LaborStats.Infrastructure.Services;

public static class CurrentUserServiceExtensions
{
    public static (Guid Id, string Name) RequireAuditUser(
        this ICurrentUserService currentUserService)
    {
        Guid? id = currentUserService.GetUserId();
        string? name = currentUserService.GetUsername();

        if (id is null || string.IsNullOrWhiteSpace(name))
        {
            throw new UnauthorizedException(
                "Authenticated user identity is missing required id/name claims.");
        }

        return (id.Value, name);
    }
}
