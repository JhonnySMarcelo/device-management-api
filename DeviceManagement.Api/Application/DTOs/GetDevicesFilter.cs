using DeviceManagementApi.Domain.Devices.Entities;

namespace DeviceManagementApi.Application.DTOs
{
    public record GetDevicesFilter
    {
        public string? Brand { get; set; }
        public DeviceState? State { get; set; }

    }
}
