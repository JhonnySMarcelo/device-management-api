using DeviceManagementApi.Application;
using DeviceManagementApi.Domain;
using DeviceManagementApi.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace DeviceManagementApi.Tests.Application.DeviceServiceTests
{
    public class DeviceService_GetAllByStateAsync_Tests
    {
        private static DeviceManagementDbContext GetDbContext()
        {
            var options = new DbContextOptionsBuilder<DeviceManagementDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

            return new DeviceManagementDbContext(options);
        }

        [Fact]
        public async Task GetAllByStateAsync_Should_Filter_By_State()
        {
            // Arrange
            var context = GetDbContext();
            var service = new DeviceService(context);
            var d1 = await service.CreateAsync("Sensor1", "BrandA");
            var d2 = await service.CreateAsync("Sensor2", "BrandB");
            d2.ChangeState(DeviceState.InUse);
            await context.SaveChangesAsync();

            // Act
            var available = await service.GetAllByStateAsync(DeviceState.Available);
            var inUse = await service.GetAllByStateAsync(DeviceState.InUse);

            // Assert
            Assert.Single(available);
            Assert.Single(inUse);
        }

        [Fact]
        public async Task GetAllByStateAsync_Should_Return_Empty_List_When_No_Matches()
        {
            var context = GetDbContext();
            var service = new DeviceService(context);
            await service.CreateAsync("Sensor1", "BrandA");
            await service.CreateAsync("Sensor2", "BrandB");

            var result = await service.GetAllByStateAsync(DeviceState.InUse);

            Assert.Empty(result);
        }

        [Fact]
        public async Task GetAllByStateAsync_Should_Return_All_Devices_Of_Same_State()
        {
            var context = GetDbContext();
            var service = new DeviceService(context);
            var d1 = await service.CreateAsync("Sensor1", "BrandA");
            var d2 = await service.CreateAsync("Sensor2", "BrandB");
            d1.ChangeState(DeviceState.InUse);
            d2.ChangeState(DeviceState.InUse);
            await context.SaveChangesAsync();

            var result = await service.GetAllByStateAsync(DeviceState.InUse);

            Assert.Equal(2, result.Count);
            Assert.All(result, d => Assert.Equal(DeviceState.InUse, d.State));
        }

    }
}
