using LaborStats.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LaborStats.Infrastructure.Data.Configurations;

public sealed class RolesConfiguration : IEntityTypeConfiguration<Roles>
{
    private static readonly Guid AdminRoleId =
        Guid.Parse("d2db6b7d-e4c2-4a50-85d1-71bd12557cf4");

    private static readonly Guid BaseUserRoleId =
        Guid.Parse("7fc7f05a-03a2-4e3e-8611-4e160ed685c1");

    public void Configure(EntityTypeBuilder<Roles> builder)
    {
        builder.ToTable("roles", "usr");

        builder.HasKey(r => r.Id);

        builder.Property(r => r.Id)
            .HasDefaultValueSql("uuidv7()");

        builder.Property(r => r.Name)
            .IsRequired()
            .HasMaxLength(50);

        builder.HasData(
            new Roles
            {
                Id = AdminRoleId,
                Name = "Admin"
            },
            new Roles
            {
                Id = BaseUserRoleId,
                Name = "BaseUser"
            });
    }
}
