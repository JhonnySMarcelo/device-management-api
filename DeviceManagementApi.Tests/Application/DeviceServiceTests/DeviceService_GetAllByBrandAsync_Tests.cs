using DeviceManagementApi.Application;
using DeviceManagementApi.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace DeviceManagementApi.Tests.Application.DeviceServiceTests
{
    public class DeviceService_GetAllByBrandAsync_Tests
    {
        private static DeviceManagementDbContext GetDbContext()
        {
            var options = new DbContextOptionsBuilder<DeviceManagementDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

            return new DeviceManagementDbContext(options);
        }

        [Fact]
        public async Task GetAllByBrandAsync_Should_Filter_By_Brand()
        {
            // Arrange
            var context = GetDbContext();
            var service = new DeviceService(context);
            await service.CreateAsync("Sensor1", "BrandA");
            await service.CreateAsync("Sensor2", "BrandB");

            // Act
            var filtered = await service.GetAllByBrandAsync("BrandA");

            // Assert
            Assert.Single(filtered);
            Assert.Equal("BrandA", filtered[0].Brand);
        }

        [Fact]
        public async Task GetAllByBrandAsync_Should_Return_All_Devices_Of_Same_Brand()
        {
            var context = GetDbContext();
            var service = new DeviceService(context);
            await service.CreateAsync("Sensor1", "BrandA");
            await service.CreateAsync("Sensor2", "BrandA");
            await service.CreateAsync("Sensor3", "BrandB");

            var result = await service.GetAllByBrandAsync("BrandA");

            Assert.Equal(2, result.Count);
            Assert.All(result, d => Assert.Equal("BrandA", d.Brand));
        }

        [Fact]
        public async Task GetAllByBrandAsync_InvalidBrand_Should_Throw()
        {
            // Arrange
            var context = GetDbContext();
            var service = new DeviceService(context);
            await service.CreateAsync("Sensor1", "BrandA");

            // Act & Assert
            await Assert.ThrowsAsync<InvalidOperationException>(
                () => service.GetAllByBrandAsync("")
            );
        }

        [Fact]
        public async Task GetAllByBrandAsync_ValidBrand_WithNoMatches_Should_Return_Empty_List()
        {
            // Arrange
            var context = GetDbContext();
            var service = new DeviceService(context);
            await service.CreateAsync("Sensor", "BrandA");

            // Act
            var result = await service.GetAllByBrandAsync("BrandB");

            // Assert
            Assert.NotNull(result);
            Assert.Empty(result);
        }
    }
}
