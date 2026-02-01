namespace DeviceManagementApi.Domain
{
    public class Device
    {
        public Guid Id { get; init; } = Guid.NewGuid();
        public string Name { get; set; }
        public string Brand { get; set; }
        public DeviceState State { get; set; } = DeviceState.Available;
        public DateTime CreationTime { get; init; } = DateTime.UtcNow;
    }
}
