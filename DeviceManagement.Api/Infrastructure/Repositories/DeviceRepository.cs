using DeviceManagementApi.Domain.Devices.Entities;
using DeviceManagementApi.Domain.Devices.Repositories;
using DeviceManagementApi.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace DeviceManagementApi.Infrastructure.Repositories
{
    public class DeviceRepository : IDeviceRepository
    {
        private readonly DeviceManagementDbContext _dbContext;

        public DeviceRepository(DeviceManagementDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<Device?> GetByIdAsync(Guid id)
                => await _dbContext.Devices.FindAsync(id);

        public IQueryable<Device> GetAll()
            => _dbContext.Devices.AsQueryable();

        public async Task AddAsync(Device device)
            => await _dbContext.Devices.AddAsync(device);

        public async Task SaveChangesAsync()
            => await _dbContext.SaveChangesAsync();

        public void Delete(Device device)
                => _dbContext.Devices.Remove(device);
    }
}
