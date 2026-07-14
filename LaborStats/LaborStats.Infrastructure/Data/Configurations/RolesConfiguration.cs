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

        builder.HasData(
            new Roles { Id = Guid.Parse("11111111-1111-1111-1111-111111111111"), Name = "Admin" },
            new Roles { Id = Guid.Parse("22222222-2222-2222-2222-222222222222"), Name = "BaseUser" }
        );
    }
}
