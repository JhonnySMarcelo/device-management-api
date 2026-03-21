using DeviceManagementApi.Application;
using DeviceManagementApi.Domain;
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

        [Fact]
        public async Task GetAllAsync_WithBrandFilter_Should_Return_OnlyMatchingDevices()
        {
            var context = GetDbContext();
            var service = new DeviceService(context);
            await service.CreateAsync("Sensor1", "Bosch");
            await service.CreateAsync("Sensor2", "Siemens");

            var result = await service.GetAllAsync("Bosch");

            Assert.Single(result);
            Assert.Equal("Bosch", result[0].Brand, ignoreCase: true);
        }

        [Fact]
        public async Task GetAllAsync_WithStateFilter_Should_Return_OnlyMatchingDevices()
        {
            var context = GetDbContext();
            var service = new DeviceService(context);
            var d1 = await service.CreateAsync("Sensor1", "Bosch");
            var d2 = await service.CreateAsync("Sensor2", "Bosch");

            // Simula mudança de estado
            d2.ChangeState(DeviceState.InUse);
            context.SaveChanges();

            var result = await service.GetAllAsync(state: DeviceState.InUse);

            Assert.Single(result);
            Assert.Equal(DeviceState.InUse, result[0].State);
        }

        [Fact]
        public async Task GetAllAsync_WithBrandAndStateFilters_Should_Return_CorrectDevices()
        {
            var context = GetDbContext();
            var service = new DeviceService(context);
            var d1 = await service.CreateAsync("Sensor1", "Bosch");
            var d2 = await service.CreateAsync("Sensor2", "Bosch");
            d2.ChangeState(DeviceState.InUse);
            context.SaveChanges();

            var result = await service.GetAllAsync("Bosch", DeviceState.InUse);

            Assert.Single(result);
            Assert.Equal(d2.Id, result[0].Id);
        }

        [Fact]
        public async Task GetAllAsync_WhenNoDevicesExist_ShouldReturnEmptyList()
        {
            var context = GetDbContext();
            var service = new DeviceService(context);

            var result = await service.GetAllAsync();

            Assert.Empty(result);
        }

        [Fact]
        public async Task GetAllAsync_WithNonExistingBrand_ShouldReturnEmptyList()
        {
            var context = GetDbContext();
            var service = new DeviceService(context);
            await service.CreateAsync("Sensor1", "Bosch");

            var result = await service.GetAllAsync("Siemens");

            Assert.Empty(result);
        }

        [Fact]
        public async Task GetAllAsync_WithBrandAndStateFilters_NoMatch_ShouldReturnEmptyList()
        {
            var context = GetDbContext();
            var service = new DeviceService(context);
            var d1 = await service.CreateAsync("Sensor1", "Bosch");
            d1.ChangeState(DeviceState.InUse);
            context.SaveChanges();

            var result = await service.GetAllAsync("Siemens", DeviceState.Available);

            Assert.Empty(result);
        }

        [Fact]
        public async Task GetAllAsync_WithBrandFilter_ShouldBeCaseInsensitive()
        {
            var context = GetDbContext();
            var service = new DeviceService(context);
            await service.CreateAsync("Sensor1", "Bosch");

            var result = await service.GetAllAsync("bosch");

            Assert.Single(result);
            Assert.Equal("Bosch", result[0].Brand, ignoreCase: true);
        }
    }
}
