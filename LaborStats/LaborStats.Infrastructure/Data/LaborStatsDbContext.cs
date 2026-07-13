using LaborStats.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace LaborStats.Infrastructure.Data;

public sealed class LaborStatsDbContext : DbContext
{
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(LaborStatsDbContext).Assembly);

        modelBuilder.HasDefaultSchema("labor_stats");

        base.OnModelCreating(modelBuilder);
    }

    public DbSet<Voivodeship> Voivodeships => Set<Voivodeship>();
    public DbSet<County> Counties => Set<County>();

    public DbSet<Professions> Profession => Set<Professions>();
    public DbSet<ProfessionGroups> ProfessionGroup => Set<ProfessionGroups>();

    public DbSet<Users> User => Set<Users>();
    public DbSet<Roles> Role => Set<Roles>();

    public LaborStatsDbContext(DbContextOptions<LaborStatsDbContext> options)
        : base(options)
    {
    }
}