namespace LaborStats.Domain.Entities;

public sealed class Professions
{
    public Guid Id { get; set; }
    public int KzisCode { get; set; }
    public string KzisName { get; set; } = null!;
    public Guid ProfessionGroupId { get; set; }
    public DateTimeOffset CreatedAt { get; set; }
    public DateTimeOffset? UpdatedAt { get; set; }
    public Guid CreatedById { get; set; }
    public string? CreatedByName { get; set; }
    public Guid? UpdatedById { get; set; }
    public string? UpdatedByName { get; set; }
    public ProfessionGroups ProfessionGroup { get; set; } = null!;
}
