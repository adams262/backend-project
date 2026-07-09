using LaborStats.Domain.Entities;
using LaborStats.Infrastructure.Data.Configurations;
using Microsoft.EntityFrameworkCore;

namespace LaborStats.Infrastructure.Data;

public sealed class LaborStatsDbContext : DbContext
{
    public DbSet<Voivodeship> Voivodeships => Set<Voivodeship>();

    public DbSet<County> Counties => Set<County>();

    public LaborStatsDbContext(DbContextOptions<LaborStatsDbContext> options)
        : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(LaborStatsDbContext).Assembly);

        base.OnModelCreating(modelBuilder);
    }
}