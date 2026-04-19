using DeviceManagementApi.Application.Services;
using DeviceManagementApi.Application.DTOs;
using DeviceManagementApi.Domain.Devices.Entities;
using DeviceManagementApi.Domain.Devices.Repositories;
using Moq;
using Xunit;

namespace DeviceManagementApi.Tests.Application.DeviceServiceTests
{
    public class DeviceService_UpdateAsync_Tests
    {
        private readonly Mock<IDeviceRepository> _mockRepo;
        private readonly DeviceService _service;

        public DeviceService_UpdateAsync_Tests()
        {
            _mockRepo = new Mock<IDeviceRepository>();
            _service = new DeviceService(_mockRepo.Object);
        }

        [Fact]
        public async Task UpdateAsync_Should_Update_Fields()
        {
            // Arrange
            var device = new Device("OldName", "OldBrand");
            _mockRepo.Setup(r => r.GetByIdAsync(device.Id))
                     .ReturnsAsync(device);

            var updateDto = new UpdateDeviceRequest
            {
                Name = "NewName",
                Brand = "NewBrand",
                State = DeviceState.InUse
            };

            // Act
            var updated = await _service.UpdateAsync(device.Id, updateDto);

            // Assert
            Assert.Equal("NewName", updated!.Name);
            Assert.Equal("NewBrand", updated.Brand);
            Assert.Equal(DeviceState.InUse, updated.State);
        }

        [Fact]
        public async Task UpdateAsync_DeviceInUse_Should_Not_Change_NameOrBrand()
        {
            var device = new Device("OldName", "OldBrand");
            device.ChangeState(DeviceState.InUse);

            _mockRepo.Setup(r => r.GetByIdAsync(device.Id))
                     .ReturnsAsync(device);

            var updateDto = new UpdateDeviceRequest
            {
                Name = "NewName",
                Brand = "NewBrand",
                State = DeviceState.Available
            };

            await Assert.ThrowsAsync<InvalidOperationException>(
                () => _service.UpdateAsync(device.Id, updateDto)
            );
        }

        [Fact]
        public async Task UpdateAsync_NonExistingDevice_Should_Return_Null()
        {
            _mockRepo.Setup(r => r.GetByIdAsync(It.IsAny<Guid>()))
                     .ReturnsAsync((Device?)null);

            var updateDto = new UpdateDeviceRequest
            {
                Name = "Name",
                Brand = "Brand",
                State = DeviceState.Available
            };

            var result = await _service.UpdateAsync(Guid.NewGuid(), updateDto);

            Assert.Null(result);
        }

        [Fact]
        public async Task UpdateAsync_OnlyStateProvided_Should_Update_State()
        {
            var device = new Device("Name", "Brand");
            _mockRepo.Setup(r => r.GetByIdAsync(device.Id))
                     .ReturnsAsync(device);

            var request = new UpdateDeviceRequest
            {
                State = DeviceState.InUse
            };

            var updated = await _service.UpdateAsync(device.Id, request);

            Assert.NotNull(updated);
            Assert.Equal(DeviceState.InUse, updated!.State);
            Assert.Equal("Name", updated.Name);
            Assert.Equal("Brand", updated.Brand);
        }

        [Fact]
        public async Task UpdateAsync_EmptyRequest_Should_Not_Change_Device()
        {
            var device = new Device("Name", "Brand");
            _mockRepo.Setup(r => r.GetByIdAsync(device.Id))
                     .ReturnsAsync(device);

            var request = new UpdateDeviceRequest();

            var updated = await _service.UpdateAsync(device.Id, request);

            Assert.Equal("Name", updated!.Name);
            Assert.Equal("Brand", updated.Brand);
            Assert.Equal(DeviceState.Available, updated.State);
        }

        [Fact]
        public async Task UpdateAsync_DeviceInUse_Should_Allow_State_Change()
        {
            var device = new Device("Name", "Brand");
            device.ChangeState(DeviceState.InUse);

            _mockRepo.Setup(r => r.GetByIdAsync(device.Id))
                     .ReturnsAsync(device);

            var request = new UpdateDeviceRequest
            {
                State = DeviceState.Available
            };

            var updated = await _service.UpdateAsync(device.Id, request);

            Assert.NotNull(updated);
            Assert.Equal(DeviceState.Available, updated!.State);
            Assert.Equal("Name", updated.Name);
            Assert.Equal("Brand", updated.Brand);
        }
    }
}
