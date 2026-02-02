using DeviceManagementApi.Domain;
using Xunit;

namespace DeviceManagementApi.Tests.Domain.DeviceTests
{
    public class Device_Constructor_Tests
    {
        [Fact]
        public void Constructor_Should_Create_Device_With_Valid_Data()
        {
            var device = new Device("Sensor", "Bosch");

            Assert.NotEqual(Guid.Empty, device.Id);
            Assert.Equal("Sensor", device.Name);
            Assert.Equal("Bosch", device.Brand);
            Assert.Equal(DeviceState.Available, device.State);
            Assert.True(device.CreationTime <= DateTime.UtcNow);
        }

        [Theory]
        [InlineData("")]
        [InlineData("   ")]
        public void Constructor_Invalid_Name_Should_Throw(string invalidName)
        {
            Assert.Throws<InvalidOperationException>(
                () => new Device(invalidName, "Brand")
            );
        }

        [Theory]
        [InlineData("")]
        [InlineData("   ")]
        public void Constructor_Invalid_Brand_Should_Throw(string invalidBrand)
        {
            Assert.Throws<InvalidOperationException>(
                () => new Device("Name", invalidBrand)
            );
        }
    }
}
