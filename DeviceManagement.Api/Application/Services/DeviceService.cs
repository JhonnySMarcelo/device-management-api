using DeviceManagementApi.Application.DTOs;
using DeviceManagementApi.Domain.Devices.Entities;
using DeviceManagementApi.Domain.Devices.Repositories;
using Microsoft.EntityFrameworkCore;

namespace DeviceManagementApi.Application.Services
{
    public class DeviceService
    {
        private readonly IDeviceRepository _repository;

        public DeviceService(IDeviceRepository repository)
        {
            _repository = repository;
        }

        public async Task<Device> CreateAsync(string name, string brand)
        {
            var device = new Device(name, brand);

            await _repository.AddAsync(device);
            await _repository.SaveChangesAsync();

            return device;
        }

        public async Task<Device?> GetByIdAsync(Guid id)
        {
            return await _repository.GetByIdAsync(id);
        }

        public async Task<List<Device>> GetAllAsync(string? brand = null, DeviceState? state = null)
        {
            var query = _repository.GetAll();

            if (!string.IsNullOrEmpty(brand))
                query = query.Where(d => d.Brand == brand);

            if (state.HasValue)
                query = query.Where(d => d.State == state.Value);

            return await query.AsNoTracking().ToListAsync();
        }

        public async Task<Device?> UpdateAsync(Guid id, UpdateDeviceRequest request)
        {
            var device = await _repository.GetByIdAsync(id);

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

            await _repository.SaveChangesAsync();
            return device;
        }

        public async Task<bool?> DeleteAsync(Guid id)
        {
            var device = await _repository.GetByIdAsync(id);
            if (device == null) return null;

            if (device.State == DeviceState.InUse)
                throw new InvalidOperationException("Device is in use and cannot be deleted.");

            _repository.Delete(device);
            await _repository.SaveChangesAsync();

            return true;
        }
    }
}
