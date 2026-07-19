using LaborStats.Infrastructure.Options;
using LaborStats.Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;

namespace LaborStats.Infrastructure.Data.Seed;

public sealed class AdminSeeder(
    LaborStatsDbContext context,
    IOptions<AdminUserOptions> options,
    IPasswordHasher<Users> passwordHasher)
{
    private readonly AdminUserOptions _options = options.Value;

    public async Task SeedAsync(
        CancellationToken cancellationToken = default)
    {
        bool userExists = await context.User
            .AnyAsync(u => u.Login == _options.Login, cancellationToken);

        if (userExists)
        {
            return;
        }

        Roles? adminRole = await context.Role
            .SingleOrDefaultAsync(role => role.Name == "Admin", cancellationToken) ?? throw new InvalidOperationException("Admin role not found in the database.");

        Users user = new()
        {
            Name = _options.Name,
            Login = _options.Login,
            Email = _options.Email,
            RoleId = adminRole.Id
        };

        user.PasswordHash = passwordHasher.HashPassword(user, _options.Password);

        context.User.Add(user);

        await context.SaveChangesAsync(cancellationToken);
    }

}
