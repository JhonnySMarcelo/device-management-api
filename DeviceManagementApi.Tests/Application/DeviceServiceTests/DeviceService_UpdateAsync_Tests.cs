using DeviceManagementApi.Application;
using DeviceManagementApi.Application.DTOs;
using DeviceManagementApi.Domain;
using DeviceManagementApi.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace DeviceManagementApi.Tests.Application.DeviceServiceTests
{
    public class DeviceService_UpdateAsync_Tests
    {
        private static DeviceManagementDbContext GetDbContext()
        {
            var options = new DbContextOptionsBuilder<DeviceManagementDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

            return new DeviceManagementDbContext(options);
        }

        [Fact]
        public async Task UpdateAsync_Should_Update_Fields()
        {
            // Arrange
            var context = GetDbContext();
            var service = new DeviceService(context);
            var device = await service.CreateAsync("OldName", "OldBrand");

            var updateDto = new UpdateDeviceRequest
            {
                Name = "NewName",
                Brand = "NewBrand",
                State = DeviceState.InUse
            };

            // Act
            var updated = await service.UpdateAsync(device.Id, updateDto);

            // Assert
            Assert.Equal("NewName", updated!.Name);
            Assert.Equal("NewBrand", updated.Brand);
            Assert.Equal(DeviceState.InUse, updated.State);
        }

        [Fact]
        public async Task UpdateAsync_DeviceInUse_Should_Not_Change_NameOrBrand()
        {
            // Arrange
            var context = GetDbContext();
            var service = new DeviceService(context);
            var device = await service.CreateAsync("OldName", "OldBrand");
            device.ChangeState(DeviceState.InUse);
            await context.SaveChangesAsync();

            var updateDto = new UpdateDeviceRequest
            {
                Name = "NewName",
                Brand = "NewBrand",
                State = DeviceState.Available
            };

            // Act & Assert
            await Assert.ThrowsAsync<InvalidOperationException>(
                () => service.UpdateAsync(device.Id, updateDto)
            );
        }

        [Fact]
        public async Task UpdateAsync_NonExistingDevice_Should_Return_Null()
        {
            // Arrange
            var context = GetDbContext();
            var service = new DeviceService(context);

            var updateDto = new UpdateDeviceRequest
            {
                Name = "Name",
                Brand = "Brand",
                State = DeviceState.Available
            };

            // Act
            var result = await service.UpdateAsync(Guid.NewGuid(), updateDto);

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public async Task UpdateAsync_OnlyStateProvided_Should_Update_State()
        {
            // Arrange
            var context = GetDbContext();
            var service = new DeviceService(context);
            var device = await service.CreateAsync("Name", "Brand");

            var request = new UpdateDeviceRequest
            {
                State = DeviceState.InUse
            };

            // Act
            var updated = await service.UpdateAsync(device.Id, request);

            // Assert
            Assert.NotNull(updated);
            Assert.Equal(DeviceState.InUse, updated!.State);
            Assert.Equal("Name", updated.Name);
            Assert.Equal("Brand", updated.Brand);
        }

        [Fact]
        public async Task UpdateAsync_EmptyRequest_Should_Not_Change_Device()
        {
            var context = GetDbContext();
            var service = new DeviceService(context);
            var device = await service.CreateAsync("Name", "Brand");

            var request = new UpdateDeviceRequest();

            var updated = await service.UpdateAsync(device.Id, request);

            Assert.Equal("Name", updated!.Name);
            Assert.Equal("Brand", updated.Brand);
            Assert.Equal(DeviceState.Available, updated.State);
        }

        [Fact]
        public async Task UpdateAsync_DeviceInUse_Should_Allow_State_Change()
        {
            var context = GetDbContext();
            var service = new DeviceService(context);
            var device = await service.CreateAsync("Name", "Brand");
            device.ChangeState(DeviceState.InUse);
            await context.SaveChangesAsync();

            var request = new UpdateDeviceRequest
            {
                State = DeviceState.Available
            };

            var updated = await service.UpdateAsync(device.Id, request);

            Assert.NotNull(updated);
            Assert.Equal(DeviceState.Available, updated!.State);
            Assert.Equal("Name", updated.Name);
            Assert.Equal("Brand", updated.Brand);
        }

    }
}
