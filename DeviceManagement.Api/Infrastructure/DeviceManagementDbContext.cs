using DeviceManagementApi.Domain;
using Microsoft.EntityFrameworkCore;

namespace DeviceManagementApi.Infrastructure
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
                      .HasMaxLength(100);

                entity.Property(d => d.Brand)
                      .IsRequired()
                      .HasMaxLength(100);

                entity.Property(d => d.State)
                      .IsRequired()
                      .HasConversion<int>();

                entity.Property(d => d.CreationTime)
                      .IsRequired();
            });
        }
    }
}
