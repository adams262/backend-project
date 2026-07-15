using LaborStats.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LaborStats.Infrastructure.Data.Configurations;

public sealed class ProfessionGroupsConfiguration : IEntityTypeConfiguration<ProfessionGroups>
{
    public void Configure(EntityTypeBuilder<ProfessionGroups> builder)
    {
        builder.ToTable("profession_groups", "prof");
        builder.HasKey(pg => pg.Id);

        builder.Property(pg => pg.Id)
            .HasDefaultValueSql("uuidv7()");

        builder.Property(pg => pg.Name)
            .HasMaxLength(255)
            .IsRequired();

        builder.Property(pg => pg.NameEn)
            .HasMaxLength(255);

        builder.Property(pg => pg.CreatedAt)
            .HasDefaultValueSql("CURRENT_TIMESTAMP")
            .IsRequired();

        builder.Property(pg => pg.CreatedById)
            .HasDefaultValue(Guid.Empty);

        builder.Property(pg => pg.CreatedByName)
            .HasMaxLength(200)
            .HasDefaultValue(string.Empty);

        builder.Property(pg => pg.UpdatedByName)
            .HasMaxLength(200);
    }
}
