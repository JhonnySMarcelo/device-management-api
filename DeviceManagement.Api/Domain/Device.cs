namespace DeviceManagementApi.Domain
{
    public class Device
    {
        public Guid Id { get; init; } = Guid.NewGuid();
        public string Name { get; private set; } = null!;
        public string Brand { get; private set; } = null!;
        public DeviceState State { get; private set; } = DeviceState.Available;
        public DateTime CreationTime { get; init; } = DateTime.UtcNow;

        protected Device() { }

        public Device(string name, string brand)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new InvalidOperationException("Device name is required.");

            if (string.IsNullOrWhiteSpace(brand))
                throw new InvalidOperationException("Device brand is required.");

            Name = name;
            Brand = brand;
        }

        public void ChangeState(DeviceState newState)
        {
            if (newState == State) return;

            State = newState;
        }

        public void ChangeName(string newName)
        {
            if (string.IsNullOrWhiteSpace(newName))
                throw new InvalidOperationException("Device name is required.");

            Name = newName;
        }

        public void ChangeBrand(string newBrand)
        {
            if (string.IsNullOrWhiteSpace(newBrand))
                throw new InvalidOperationException("Device brand is required.");

            Brand = newBrand;
        }
    }
}
