using DeviceManagementApi.Application;
using DeviceManagementApi.Domain;
using DeviceManagementApi.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace DeviceManagementApi.Tests.Application.DeviceServiceTests
{
    public class DeviceService_DeleteAsync_Tests
    {
        private static DeviceManagementDbContext GetDbContext()
        {
            var options = new DbContextOptionsBuilder<DeviceManagementDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

            return new DeviceManagementDbContext(options);
        }

        [Fact]
        public async Task DeleteAsync_Should_Delete_Device()
        {
            // Arrange
            var context = GetDbContext();
            var service = new DeviceService(context);
            var device = await service.CreateAsync("Name", "Brand");

            // Act
            var result = await service.DeleteAsync(device.Id);

            // Assert
            Assert.True(result);
            Assert.Null(await service.GetByIdAsync(device.Id));
        }

        [Fact]
        public async Task DeleteAsync_DeviceInUse_Should_Throw()
        {
            // Arrange
            var context = GetDbContext();
            var service = new DeviceService(context);
            var device = await service.CreateAsync("Name", "Brand");
            device.ChangeState(DeviceState.InUse);
            await context.SaveChangesAsync();

            // Act & Assert
            await Assert.ThrowsAsync<InvalidOperationException>(
                () => service.DeleteAsync(device.Id)
            );
        }

        [Fact]
        public async Task DeleteAsync_NonExistingDevice_Should_Return_Null()
        {
            // Arrange
            var context = GetDbContext();
            var service = new DeviceService(context);

            // Act
            var result = await service.DeleteAsync(Guid.NewGuid());

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public async Task DeleteAsync_InactiveDevice_Should_Delete()
        {
            // Arrange
            var context = GetDbContext();
            var service = new DeviceService(context);
            var device = await service.CreateAsync("Name", "Brand");
            device.ChangeState(DeviceState.Inactive);
            await context.SaveChangesAsync();

            // Act
            var result = await service.DeleteAsync(device.Id);

            // Assert
            Assert.True(result);
            Assert.Null(await service.GetByIdAsync(device.Id));
        }

    }
}
