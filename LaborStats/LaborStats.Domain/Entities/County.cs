namespace LaborStats.Domain.Entities;

public sealed class County
{
    public string Teryt { get; set; } = null!;

    public string Name { get; set; } = null!;

    public bool IsCityWithCountyRights { get; set; }

    public string VoivodeshipTeryt { get; set; } = null!;

    public Voivodeship Voivodeship { get; set; } = null!;
}