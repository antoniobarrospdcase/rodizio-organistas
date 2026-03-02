using Microsoft.EntityFrameworkCore;
using RodizioOrganistas.Domain.Entities;
using RodizioOrganistas.Domain.Enums;

namespace RodizioOrganistas.Infrastructure.Data;

public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
{
    public DbSet<Church> Churches => Set<Church>();
    public DbSet<ChurchServiceDay> ChurchServiceDays => Set<ChurchServiceDay>();
    public DbSet<Organist> Organists => Set<Organist>();
    public DbSet<OrganistAvailability> OrganistAvailabilities => Set<OrganistAvailability>();
    public DbSet<AppUser> Users => Set<AppUser>();
    public DbSet<Schedule> Schedules => Set<Schedule>();
    public DbSet<ScheduleItem> ScheduleItems => Set<ScheduleItem>();

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

        modelBuilder.Entity<AppUser>(e =>
        {
            e.Property(x => x.Username).HasMaxLength(100).IsRequired();
            e.HasIndex(x => x.Username).IsUnique();
            e.Property(x => x.Password).HasMaxLength(200).IsRequired();
            e.Property(x => x.Role).HasConversion<int>();
        });

        modelBuilder.Entity<Schedule>(e =>
        {
            e.Property(x => x.ServiceType).HasConversion<int>();
            e.HasOne(x => x.Church).WithMany().HasForeignKey(x => x.ChurchId).OnDelete(DeleteBehavior.Cascade);
            e.HasOne(x => x.CreatedByUser).WithMany().HasForeignKey(x => x.CreatedByUserId).OnDelete(DeleteBehavior.Restrict);
        });

        modelBuilder.Entity<ScheduleItem>(e =>
        {
            e.Property(x => x.OrganistName).HasMaxLength(100).IsRequired();
            e.Property(x => x.HalfHourOrganistName).HasMaxLength(100);
            e.HasOne(x => x.Schedule).WithMany(x => x.Items).HasForeignKey(x => x.ScheduleId).OnDelete(DeleteBehavior.Cascade);
        });

        modelBuilder.Entity<AppUser>().HasData(new AppUser
        {
            Id = 1,
            Username = "master",
            Password = "Master@123",
            Role = UserRole.MasterAdmin
        });
    }
}
