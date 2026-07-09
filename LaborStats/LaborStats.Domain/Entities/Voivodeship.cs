namespace LaborStats.Domain.Entities;

public sealed class Voivodeship
{
    public string Teryt { get; set; } = null!;

    public string Name { get; set; } = null!;

    public ICollection<County> Counties { get; set; } = [];
}