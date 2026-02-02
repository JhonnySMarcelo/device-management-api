using DeviceManagementApi.Application;
using DeviceManagementApi.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace DeviceManagementApi.Tests.Application.DeviceServiceTests
{
    public class DeviceService_GetAllAsync_Tests
    {
        private static DeviceManagementDbContext GetDbContext()
        {
            var options = new DbContextOptionsBuilder<DeviceManagementDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

            return new DeviceManagementDbContext(options);
        }

        [Fact]
        public async Task GetAllAsync_Should_Return_All_Devices()
        {
            // Arrange
            var context = GetDbContext();
            var service = new DeviceService(context);
            await service.CreateAsync("Sensor1", "BrandA");
            await service.CreateAsync("Sensor2", "BrandB");

            // Act
            var all = await service.GetAllAsync();

            // Assert
            Assert.Equal(2, all.Count);
        }

        [Fact]
        public async Task GetAllAsync_Should_Return_Correct_Devices()
        {
            var context = GetDbContext();
            var service = new DeviceService(context);
            var d1 = await service.CreateAsync("Sensor1", "BrandA");
            var d2 = await service.CreateAsync("Sensor2", "BrandB");

            var all = await service.GetAllAsync();

            Assert.Contains(all, d => d.Id == d1.Id && d.Name == "Sensor1" && d.Brand == "BrandA");
            Assert.Contains(all, d => d.Id == d2.Id && d.Name == "Sensor2" && d.Brand == "BrandB");
        }

    }
}
