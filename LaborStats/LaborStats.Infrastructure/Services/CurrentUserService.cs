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
}