using DeviceManagementApi.Application.DTOs;
using DeviceManagementApi.Domain;
using DeviceManagementApi.Infrastructure;
using Microsoft.EntityFrameworkCore;

namespace DeviceManagementApi.Application
{
    public class DeviceService
    {
        private readonly DeviceManagementDbContext _dbContext;

        public DeviceService(DeviceManagementDbContext dbContext) 
        {
            _dbContext = dbContext;
        }

        public async Task<Device> CreateAsync(string name, string brand)
        {
            var device = new Device(name, brand);

            _dbContext.Devices.Add(device);
            await _dbContext.SaveChangesAsync();

            return device;
        }

        public async Task<Device?> GetByIdAsync(Guid id)
        {
            return await _dbContext.Devices
                .FirstOrDefaultAsync(d => d.Id == id);
        }

        public async Task<List<Device>> GetAllAsync()
        {
            return await _dbContext.Devices.ToListAsync();
        }

        public async Task<List<Device>> GetAllByBrandAsync(string brand)
        {
            if (string.IsNullOrWhiteSpace(brand))
                throw new InvalidOperationException("Brand is required.");

            return await _dbContext.Devices
                .Where(d => d.Brand == brand)
                .ToListAsync();
        }
    }
}
