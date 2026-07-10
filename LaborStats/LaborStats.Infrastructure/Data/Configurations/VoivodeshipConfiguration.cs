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
    }
}