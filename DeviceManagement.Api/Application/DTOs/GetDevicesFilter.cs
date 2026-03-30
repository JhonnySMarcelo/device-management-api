using DeviceManagementApi.Domain;

namespace DeviceManagementApi.Application.DTOs
{
    public record GetDevicesFilter
    {
        public string? Brand { get; set; }
        public DeviceState? State { get; set; }

    }
}
