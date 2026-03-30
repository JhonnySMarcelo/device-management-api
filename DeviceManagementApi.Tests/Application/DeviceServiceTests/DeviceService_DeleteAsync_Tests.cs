using DeviceManagementApi.Application.Services;
using DeviceManagementApi.Domain.Devices.Entities;
using DeviceManagementApi.Domain.Devices.Repositories;
using Moq;
using Xunit;

namespace DeviceManagementApi.Tests.Application.DeviceServiceTests
{
    public class DeviceService_DeleteAsync_Tests
    {
        private readonly Mock<IDeviceRepository> _mockRepo;
        private readonly DeviceService _service;

        public DeviceService_DeleteAsync_Tests()
        {
            _mockRepo = new Mock<IDeviceRepository>();
            _service = new DeviceService(_mockRepo.Object);
        }

        [Fact]
        public async Task DeleteAsync_Should_Delete_Device()
        {
            // Arrange
            var device = new Device("Name", "Brand");
            _mockRepo.Setup(r => r.GetByIdAsync(device.Id))
                     .ReturnsAsync(device);

            // Act
            var result = await _service.DeleteAsync(device.Id);

            // Assert
            Assert.True(result);
            _mockRepo.Verify(r => r.Delete(device), Times.Once);
            _mockRepo.Verify(r => r.SaveChangesAsync(), Times.Once);
        }

        [Fact]
        public async Task DeleteAsync_DeviceInUse_Should_Throw()
        {
            // Arrange
            var device = new Device("Name", "Brand");
            device.ChangeState(DeviceState.InUse);
            _mockRepo.Setup(r => r.GetByIdAsync(device.Id))
                     .ReturnsAsync(device);

            // Act & Assert
            await Assert.ThrowsAsync<InvalidOperationException>(
                () => _service.DeleteAsync(device.Id)
            );
        }

        [Fact]
        public async Task DeleteAsync_NonExistingDevice_Should_Return_Null()
        {
            // Arrange
            _mockRepo.Setup(r => r.GetByIdAsync(It.IsAny<Guid>()))
                     .ReturnsAsync((Device?)null);

            // Act
            var result = await _service.DeleteAsync(Guid.NewGuid());

            // Assert
            Assert.Null(result);
            _mockRepo.Verify(r => r.Delete(It.IsAny<Device>()), Times.Never);
            _mockRepo.Verify(r => r.SaveChangesAsync(), Times.Never);
        }

        [Fact]
        public async Task DeleteAsync_InactiveDevice_Should_Delete()
        {
            // Arrange
            var device = new Device("Name", "Brand");
            device.ChangeState(DeviceState.Inactive);
            _mockRepo.Setup(r => r.GetByIdAsync(device.Id))
                     .ReturnsAsync(device);

            // Act
            var result = await _service.DeleteAsync(device.Id);

            // Assert
            Assert.True(result);
            _mockRepo.Verify(r => r.Delete(device), Times.Once);
            _mockRepo.Verify(r => r.SaveChangesAsync(), Times.Once);
        }
    }
}
