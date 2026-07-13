namespace LaborStats.Domain.Entities;

public sealed class Roles
{
    public Guid Id { get; set; }
    public string Name { get; set; } = null!;
    public ICollection<Users> User { get; set; } = [];
}

