using LaborStats.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LaborStats.Infrastructure.Data.Configurations;

public sealed class CountyConfiguration : IEntityTypeConfiguration<County>
{
    public void Configure(EntityTypeBuilder<County> builder)
    {
        builder.ToTable("counties");
        builder.HasKey(c => c.Teryt);

        builder.Property(c => c.Teryt)
            .HasMaxLength(4)
            .IsFixedLength();

        builder.Property(c => c.Name)
            .HasMaxLength(64)
            .IsRequired();

        builder.Property(c => c.IsCityWithCountyRights)
            .IsRequired();

        builder.Property(c => c.VoivodeshipTeryt)
            .HasMaxLength(2)
            .IsFixedLength()
            .IsRequired();

        builder.HasOne(c => c.Voivodeship)
            .WithMany(v => v.Counties)
            .HasForeignKey(c => c.VoivodeshipTeryt)
            .OnDelete(DeleteBehavior.Restrict);
    }
}