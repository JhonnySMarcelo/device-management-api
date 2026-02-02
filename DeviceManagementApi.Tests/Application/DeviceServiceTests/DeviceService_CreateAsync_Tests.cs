using DeviceManagementApi.Application;
using DeviceManagementApi.Domain;
using DeviceManagementApi.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace DeviceManagementApi.Tests.Application.DeviceServiceTests
{
    public class DeviceService_CreateAsync_Tests
    {
        private static DeviceManagementDbContext GetDbContext()
        {
            var options = new DbContextOptionsBuilder<DeviceManagementDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

            return new DeviceManagementDbContext(options);
        }

        [Fact]
        public async Task CreateAsync_Should_Create_Device()
        {
            //Arrange
            var context = GetDbContext();
            var service = new DeviceService(context);

            //Act
            var device = await service.CreateAsync("Sensor", "Bosch");

            //Assert
            Assert.NotNull(device);
            Assert.Equal("Sensor", device.Name);
            Assert.Equal("Bosch", device.Brand);
            Assert.Equal(DeviceState.Available, device.State);
            Assert.NotEqual(Guid.Empty, device.Id);
        }

        [Fact]
        public async Task Should_Persist_Device_In_Database()
        {
            // Arrange
            var context = GetDbContext();
            var service = new DeviceService(context);

            // Act
            var device = await service.CreateAsync("Sensor", "Bosch");

            // Assert
            var stored = await context.Devices.FindAsync(device.Id);
            Assert.NotNull(stored);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("   ")]
        public async Task Should_Throw_InvalidOperationException_When_Name_Is_Invalid(string? invalidName)
        {
            // Arrange
            var context = GetDbContext();
            var service = new DeviceService(context);

            // Act & Assert
            await Assert.ThrowsAsync<InvalidOperationException>(
                () => service.CreateAsync(invalidName!, "Brand")
            );
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("   ")]
        public async Task Should_Throw_InvalidOperationException_When_Brand_Is_Invalid(string? invalidBrand)
        {
            // Arrange
            var context = GetDbContext();
            var service = new DeviceService(context);

            // Act & Assert
            await Assert.ThrowsAsync<InvalidOperationException>(
                () => service.CreateAsync("Name", invalidBrand!)
            );
        }
    }

}
