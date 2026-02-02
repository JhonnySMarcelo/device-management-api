using DeviceManagementApi.Domain;
using Xunit;

namespace DeviceManagementApi.Tests.Domain.DeviceTests
{
    public class Device_ChangeBrand_Tests
    {
        [Fact]
        public void ChangeBrand_Should_Update_Brand()
        {
            var device = new Device("Name", "OldBrand");

            device.ChangeBrand("NewBrand");

            Assert.Equal("NewBrand", device.Brand);
        }

        [Theory]
        [InlineData("")]
        [InlineData("   ")]
        public void ChangeBrand_Invalid_Value_Should_Throw(string invalidBrand)
        {
            var device = new Device("Name", "Brand");

            Assert.Throws<InvalidOperationException>(
                () => device.ChangeBrand(invalidBrand)
            );
        }

        [Fact]
        public void ChangeBrand_Same_Brand_Should_Update()
        {
            var device = new Device("Name", "Brand");

            device.ChangeBrand("Brand");

            Assert.Equal("Brand", device.Brand);
        }

    }
}
