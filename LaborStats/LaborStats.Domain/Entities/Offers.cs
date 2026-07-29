namespace LaborStats.Domain.Entities;

public sealed class Offers
{
    public Guid Id { get; set; }
    public int Year { get; set; }
    public PeriodType Period { get; set; }
    public Guid ProfessionGroupId { get; set; }
    public ProfessionGroups ProfessionGroup { get; set; } = null!;
    public string SnapshotCompanyName { get; set; } = null!;
    public DateOnly? SnapshotExpirationDate { get; set; }
    public string SnapshotJobTitle { get; set; } = null!;
    public DateOnly SnapshotPublicationDate { get; set; }
}
