using DeviceManagementApi.Application;
using DeviceManagementApi.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace DeviceManagementApi.Tests.Application.DeviceServiceTests
{
    public class DeviceService_GetByIdAsync_Tests
    {
        private static DeviceManagementDbContext GetDbContext()
        {
            var options = new DbContextOptionsBuilder<DeviceManagementDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

            return new DeviceManagementDbContext(options);
        }

        [Fact]
        public async Task GetByIdAsync_Should_Return_Device()
        {
            // Arrange
            var context = GetDbContext();
            var service = new DeviceService(context);
            var created = await service.CreateAsync("Sensor", "Bosch");

            // Act
            var fetched = await service.GetByIdAsync(created.Id);

            // Assert
            Assert.NotNull(fetched);
            Assert.Equal(created.Id, fetched!.Id);
        }

        [Fact]
        public async Task GetByIdAsync_InvalidId_Should_Return_Null()
        {
            // Arrange
            var context = GetDbContext();
            var service = new DeviceService(context);

            // Act
            var fetched = await service.GetByIdAsync(Guid.NewGuid());

            // Assert
            Assert.Null(fetched);
        }

        [Fact]
        public async Task GetByIdAsync_EmptyGuid_Should_Return_Null()
        {
            // Arrange
            var context = GetDbContext();
            var service = new DeviceService(context);

            // Act
            var result = await service.GetByIdAsync(Guid.Empty);

            // Assert
            Assert.Null(result);
        }

    }
}
