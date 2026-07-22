using System;
using System.Collections.Generic;
using System.Text;
using LaborStats.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LaborStats.Infrastructure.Data.Configurations
{
    public sealed class ImportHistoryConfiguration : IEntityTypeConfiguration<ImportHistory>
    {
        public void Configure(EntityTypeBuilder<ImportHistory> builder)
        {
            builder.ToTable("import_histories", "import");

            builder.HasKey(x => x.Id);

            builder.Property(x => x.Id)
                .HasDefaultValueSql("uuidv7()");

            builder.Property(x => x.FileName)
                .HasMaxLength(255)
                .IsRequired();

            builder.Property(x => x.Voivodeship)
                .HasMaxLength(100)
                .IsRequired();

            builder.Property(x => x.Period)
                .HasMaxLength(50)
                .IsRequired();

            builder.Property(x => x.ImportEndDate)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .IsRequired();

            builder.Property(x => x.CreatedBy)
                .HasMaxLength(200)
                .IsRequired();
        }
    }
}
