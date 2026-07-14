using LaborStats.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LaborStats.Infrastructure.Data.Configurations;

public sealed class RolesConfiguration : IEntityTypeConfiguration<Roles>
{
    public void Configure(EntityTypeBuilder<Roles> builder)
    {
        builder.ToTable("roles", "usr");

        builder.HasKey(r => r.Id);

        builder.Property(r => r.Id)
            .HasDefaultValueSql("uuidv7()");

        builder.Property(r => r.Name)
            .IsRequired()
            .HasMaxLength(50);
    }
}
