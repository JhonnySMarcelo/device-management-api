namespace DeviceManagementApi.Application.DTOs
{
    public record CreateDeviceRequest
    {
        public required string Name { get; init; }
        public required string Brand { get; init; }
    }
}
