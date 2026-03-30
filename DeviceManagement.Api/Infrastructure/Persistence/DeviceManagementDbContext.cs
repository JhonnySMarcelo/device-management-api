using DeviceManagementApi.Domain.Devices.Entities;
using Microsoft.EntityFrameworkCore;

namespace DeviceManagementApi.Infrastructure.Persistence
{
    public class DeviceManagementDbContext : DbContext
    {
        public DeviceManagementDbContext(
            DbContextOptions<DeviceManagementDbContext> options
            ) : base (options) { }

        public DbSet<Device> Devices => Set<Device>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Device>(entity =>
            {
                entity.ToTable("Devices");

                entity.HasKey(e => e.Id);

                entity.Property(d => d.Name)
                      .IsRequired()
                      .HasMaxLength(100)
                      .UseCollation("SQL_Latin1_General_CP1_CI_AS");

                entity.Property(d => d.Brand)
                      .IsRequired()
                      .HasMaxLength(100)
                      .UseCollation("SQL_Latin1_General_CP1_CI_AS");

                entity.Property(d => d.State)
                      .IsRequired()
                      .HasConversion<int>();

                entity.Property(d => d.CreationTime)
                      .IsRequired();

                entity.HasIndex(d => d.Brand);
                entity.HasIndex(d => d.State);
            });
        }
    }
}
