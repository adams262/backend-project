using LaborStats.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LaborStats.Infrastructure.Data.Configurations;

public sealed class  ProfessionsConfiguration : IEntityTypeConfiguration<Professions>
{
    public void Configure(EntityTypeBuilder<Professions> builder)
    {
        builder.ToTable("professions", "prof");
        builder.HasKey(p => p.Id);

        builder.Property(p => p.Id)
            .HasDefaultValueSql("uuidv7()");

        builder.Property(p => p.KzisCode)
            .IsRequired();

        builder.Property(p => p.KzisName)
            .HasMaxLength(255)
            .IsRequired();

        builder.Property(p => p.CreatedAt)
            .HasDefaultValueSql("CURRENT_TIMESTAMP")
            .IsRequired();

        builder.Property(p => p.CreatedById)
            .HasDefaultValue(Guid.Empty);

        builder.Property(p => p.CreatedByName)
            .HasMaxLength(200)
            .HasDefaultValue(string.Empty);

        builder.Property(p => p.UpdatedByName)
            .HasMaxLength(200);

        builder.HasOne(p => p.ProfessionGroup)
            .WithMany(pg => pg.Professions)
            .HasForeignKey(p => p.ProfessionGroupId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}

