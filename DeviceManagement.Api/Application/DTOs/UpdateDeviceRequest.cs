using DeviceManagementApi.Domain;

namespace DeviceManagementApi.Application.DTOs
{
    public record UpdateDeviceRequest
    {
        public string? Name { get; init; }
        public string? Brand { get; init; }
        public DeviceState? State { get; init; }
    }
}
