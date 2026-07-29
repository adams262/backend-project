using System.Security.Claims;
using LaborStats.Application.Abstractions;
using Microsoft.AspNetCore.Http;

namespace LaborStats.Infrastructure.Services;

public class CurrentUserService(IHttpContextAccessor httpContextAccessor) : ICurrentUserService
{
    public string? GetUsername()
    {
        var user = httpContextAccessor.HttpContext?.User;

        return user?.FindFirstValue(ClaimTypes.Name)
            ?? user?.FindFirstValue(ClaimTypes.Email);
    }

    public Guid? GetUserId()
    {
        var value = httpContextAccessor.HttpContext?.User?.FindFirstValue(ClaimTypes.NameIdentifier);

        return value is not null && Guid.TryParse(value, out var id) ? id : null;
    }
}