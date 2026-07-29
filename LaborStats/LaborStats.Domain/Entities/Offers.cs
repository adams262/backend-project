namespace LaborStats.Domain.Entities;

public sealed class Offers
{
    public Guid Id { get; set; }
    public int Year { get; set; }
    public PeriodType Period { get; set; }
    public Guid ProfessionGroupId { get; set; }
    public ProfessionGroups ProfessionGroup { get; set; } = null!;
    public string CompanyName { get; set; } = null!;
    public DateOnly? ExpirationDate { get; set; }
    public string JobTitle { get; set; } = null!;
    public DateOnly PublicationDate { get; set; }
}
