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

        public async Task<List<Device>> GetAllByStateAsync(DeviceState state)
        {
            return await _dbContext.Devices
                .Where(d => d.State == state)
                .ToListAsync();
        }

        public async Task<Device?> UpdateAsync(Guid id, UpdateDeviceRequest request)
        {
            var device = await _dbContext.Devices.FindAsync(id);

            if (device == null) return null;

            bool isInUse = device.State == DeviceState.InUse;

            if (isInUse &&
                (!string.IsNullOrWhiteSpace(request.Name) || !string.IsNullOrWhiteSpace(request.Brand)))
            {
                throw new InvalidOperationException("Device is in use: Name and Brand cannot be changed.");
            }
            else
            {
                if (!string.IsNullOrWhiteSpace(request.Name))
                    device.ChangeName(request.Name);

                if (!string.IsNullOrWhiteSpace(request.Brand))
                    device.ChangeBrand(request.Brand);
            }

            if (request.State.HasValue)
                device.ChangeState(request.State.Value);

            await _dbContext.SaveChangesAsync();
            return device;
        }

        public async Task<bool?> DeleteAsync(Guid id)
        {
            var device = await _dbContext.Devices.FindAsync(id);
            if (device == null) return null;

            if (device.State == DeviceState.InUse)
                throw new InvalidOperationException("Device is in use and cannot be deleted.");

            _dbContext.Devices.Remove(device);
            await _dbContext.SaveChangesAsync();

            return true;
        }
    }
}
