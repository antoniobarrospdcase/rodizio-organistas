using Microsoft.EntityFrameworkCore;
using RodizioOrganistas.Domain.Entities;

namespace RodizioOrganistas.Infrastructure.Data;

public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
{
    public DbSet<Church> Churches => Set<Church>();
    public DbSet<ChurchServiceDay> ChurchServiceDays => Set<ChurchServiceDay>();
    public DbSet<Organist> Organists => Set<Organist>();
    public DbSet<OrganistAvailability> OrganistAvailabilities => Set<OrganistAvailability>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Church>(e =>
        {
            e.Property(p => p.Name).HasMaxLength(200).IsRequired();
            e.Property(p => p.City).HasMaxLength(120).IsRequired();
        });

        modelBuilder.Entity<Organist>(e =>
        {
            e.Property(p => p.Name).HasMaxLength(200).IsRequired();
            e.Property(p => p.ShortName).HasMaxLength(50).IsRequired();
            e.Property(p => p.Phone).HasMaxLength(20).IsRequired();
        });
    }
}
