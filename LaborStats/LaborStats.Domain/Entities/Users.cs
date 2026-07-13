namespace LaborStats.Domain.Entities;

public sealed class Users
{
    public Guid Id { get; set; }
    public string Name { get; set; } = null!;
    public string Login { get; set; } = null!;
    public string Email { get; set; } = null!;
    public string PasswordHash { get; set; } = null!;
    public Guid RoleId { get; set; }
    public Roles Role { get; set; } = null!;
}

