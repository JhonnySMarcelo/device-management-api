using DeviceManagementApi.Domain.Devices.Entities;

namespace DeviceManagementApi.Domain.Devices.Repositories
{
    public interface IDeviceRepository
    {
        Task<Device?> GetByIdAsync(Guid id);
        IQueryable<Device> GetAll();
        Task AddAsync(Device device);
        Task SaveChangesAsync();
        void Delete(Device device);
    }
}
