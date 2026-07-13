namespace LaborStats.Domain.Entities;

public sealed class ProfessionGroups
{
    public Guid Id { get; set; }
    public string Name { get; set; } = null!;
    public string? NameEn { get; set; }
    public DateTimeOffset CreatedAt { get; set; }
    public DateTimeOffset? UpdatedAt { get; set; }
    public Guid CreatedById { get; set; }
    public string? CreatedByName { get; set; }
    public Guid? UpdatedById { get; set; }
    public string? UpdatedByName { get; set; } = null!;
    public ICollection<Professions> Professions { get; set; } = [];
}

