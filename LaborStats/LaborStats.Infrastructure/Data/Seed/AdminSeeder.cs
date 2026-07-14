using LaborStats.Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace LaborStats.Infrastructure.Data.Seed;

public sealed class AdminSeeder
{
    private readonly LaborStatsDbContext _context;
    private readonly IConfiguration _configuration;

    public AdminSeeder(LaborStatsDbContext context, IConfiguration configuration)
    {
        _context = context;
        _configuration = configuration;
    }

    public async Task SeedAsync()
    {
        string? login = _configuration["AdminUser:Login"];

        if (await _context.User.AnyAsync(u => u.Login == login))
            return;

        Roles adminRole = await _context.Role.SingleAsync(r => r.Name == "Admin");

        var user = new Users
        {
            Name = _configuration["AdminUser:Name"]!,
            Login = login!,
            Email = _configuration["AdminUser:Email"]!,
            RoleId = adminRole.Id,
        };

        var hasher = new PasswordHasher<Users>();

        user.PasswordHash = hasher.HashPassword(user, _configuration["AdminUser:Password"]!);

        _context.User.Add(user);

        await _context.SaveChangesAsync();
    }

}
