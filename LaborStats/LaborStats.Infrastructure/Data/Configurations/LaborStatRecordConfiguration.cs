using System;
using System.Collections.Generic;
using System.Text;
using LaborStats.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LaborStats.Infrastructure.Data.Configurations
{
    public sealed class LaborStatRecordConfiguration : IEntityTypeConfiguration<LaborStatRecord>
    {
        public void Configure(EntityTypeBuilder<LaborStatRecord> builder)
        {
            builder.ToTable("labor_stat_records", "import");

            builder.HasKey(x => x.Id);

            builder.Property(x => x.Id)
                .HasDefaultValueSql("uuidv7()");

            builder.Property(x => x.Voivodeship)
                .HasMaxLength(100);

            builder.Property(x => x.County)
                .HasMaxLength(100);

            builder.Property(x => x.OccupationCode)
                .HasMaxLength(50);

            builder.Property(x => x.InsuranceTitleCode)
                .HasMaxLength(50);

            builder.Property(x => x.DataType)
                .HasMaxLength(50)
                .IsRequired();

            builder.HasOne(x => x.ImportHistory)
                   .WithMany()
                   .HasForeignKey(x => x.ImportHistoryId)
                   .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
