using Microsoft.EntityFrameworkCore;
using Monitoring.Domain.Devices;
using Monitoring.Domain.Ping;

namespace Monitoring.Infrastructure.Persistence;

public sealed class MonitoringDbContext : DbContext
{
    public MonitoringDbContext(DbContextOptions<MonitoringDbContext> options)
        : base(options)
    {
    }

    public DbSet<Device> Devices => Set<Device>();

    public DbSet<PingResult> PingResults => Set<PingResult>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Device>(entity =>
        {
            entity.ToTable("Devices");

            entity.HasKey(x => x.Id);

            entity.Property(x => x.Name)
                  .HasMaxLength(200)
                  .IsRequired();

            entity.Property(x => x.IpAddress)
                  .HasMaxLength(50)
                  .IsRequired();

            entity.Property(x => x.IsEnabled)
                  .IsRequired();
        });

        modelBuilder.Entity<PingResult>(entity =>
        {
            entity.ToTable("PingResults");

            entity.HasKey(x => x.Id);

            entity.Property(x => x.IpAddress)
                  .HasMaxLength(50)
                  .IsRequired();

            entity.Property(x => x.Status)
                  .HasConversion<int>()
                  .IsRequired();

            entity.Property(x => x.ResponseTimeMs)
                  .IsRequired();

            entity.Property(x => x.ErrorMessage)
                  .HasMaxLength(1000);

            entity.Property(x => x.CheckedAtUtc)
                  .IsRequired();
        });
    }
}