using LaborStats.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LaborStats.Infrastructure.Data.Configurations;

public sealed class VoivodeshipConfiguration : IEntityTypeConfiguration<Voivodeship>
{
    public void Configure(EntityTypeBuilder<Voivodeship> builder)
    {
        builder.ToTable("voivodeships", "geo");
        builder.HasKey(v => v.Teryt);

        builder.Property(v => v.Teryt)
            .HasMaxLength(2)
            .IsFixedLength();

        builder.Property(v => v.Name)
            .HasMaxLength(64)
            .IsRequired();


        builder.HasData(
            new Voivodeship { Teryt = "02", Name = "DOLNOŚLĄSKIE" },
            new Voivodeship { Teryt = "04", Name = "KUJAWSKO-POMORSKIE" },
            new Voivodeship { Teryt = "06", Name = "LUBELSKIE" },
            new Voivodeship { Teryt = "08", Name = "LUBUSKIE" },
            new Voivodeship { Teryt = "10", Name = "ŁÓDZKIE" },
            new Voivodeship { Teryt = "12", Name = "MAŁOPOLSKIE" },
            new Voivodeship { Teryt = "14", Name = "MAZOWIECKIE" },
            new Voivodeship { Teryt = "16", Name = "OPOLSKIE" },
            new Voivodeship { Teryt = "18", Name = "PODKARPACKIE" },
            new Voivodeship { Teryt = "20", Name = "PODLASKIE" },
            new Voivodeship { Teryt = "22", Name = "POMORSKIE" },
            new Voivodeship { Teryt = "24", Name = "ŚLĄSKIE" },
            new Voivodeship { Teryt = "26", Name = "ŚWIĘTOKRZYSKIE" },
            new Voivodeship { Teryt = "28", Name = "WARMIŃSKO-MAZURSKIE" },
            new Voivodeship { Teryt = "30", Name = "WIELKOPOLSKIE" },
            new Voivodeship { Teryt = "32", Name = "ZACHODNIOPOMORSKIE" }
        );
    }
}
