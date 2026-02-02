using DeviceManagementApi.Domain;
using Xunit;

namespace DeviceManagementApi.Tests.Domain.DeviceTests
{
    public class Device_ChangeName_Tests
    {
        [Fact]
        public void ChangeName_Should_Update_Name()
        {
            var device = new Device("Old", "Brand");

            device.ChangeName("New");

            Assert.Equal("New", device.Name);
        }

        [Theory]
        [InlineData("")]
        [InlineData("   ")]
        public void ChangeName_Invalid_Value_Should_Throw(string invalidName)
        {
            var device = new Device("Name", "Brand");

            Assert.Throws<InvalidOperationException>(
                () => device.ChangeName(invalidName)
            );
        }

        [Fact]
        public void ChangeName_Same_Name_Should_Update()
        {
            var device = new Device("Name", "Brand");

            device.ChangeName("Name");

            Assert.Equal("Name", device.Name);
        }

    }
}
