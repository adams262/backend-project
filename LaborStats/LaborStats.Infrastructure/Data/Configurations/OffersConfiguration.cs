using LaborStats.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LaborStats.Infrastructure.Data.Configurations;

public sealed class OffersConfiguration : IEntityTypeConfiguration<Offers>
{
    public void Configure(EntityTypeBuilder<Offers> builder)
    {
        builder.ToTable("offers","off");

        builder.HasKey(o => o.Id);

        builder.Property(o => o.Id)
            .HasDefaultValueSql("uuidv7()");

        builder.Property(o => o.Year)
            .IsRequired();

        builder.Property(o => o.Period)
            .HasConversion<byte>()
            .IsRequired();

        builder.Property(o => o.ProfessionGroupId)
            .IsRequired();

        builder.Property(o => o.CompanyName)
            .HasMaxLength(500)
            .IsRequired();

        builder.Property(o => o.ExpirationDate)
            .HasColumnType("date");

        builder.Property(o => o.JobTitle)
            .HasMaxLength(500)
            .IsRequired();

        builder.Property(o => o.PublicationDate)
            .HasColumnType("date")
            .IsRequired();

        builder.HasOne(o => o.ProfessionGroup)
            .WithMany()
            .HasForeignKey(o => o.ProfessionGroupId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasIndex(o => o.ProfessionGroupId);
        builder.HasIndex(o => new { o.Year, o.Period});
        builder.HasIndex(o => o.PublicationDate);
        builder.HasIndex(o => o.ExpirationDate);
    }
}
