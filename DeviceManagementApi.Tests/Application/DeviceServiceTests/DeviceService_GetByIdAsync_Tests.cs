using DeviceManagementApi.Application.Services;
using DeviceManagementApi.Domain.Devices.Entities;
using DeviceManagementApi.Domain.Devices.Repositories;
using Moq;
using Xunit;

namespace DeviceManagementApi.Tests.Application.DeviceServiceTests
{
    public class DeviceService_GetByIdAsync_Tests
    {
        private readonly Mock<IDeviceRepository> _mockRepo;
        private readonly DeviceService _service;

        public DeviceService_GetByIdAsync_Tests()
        {
            _mockRepo = new Mock<IDeviceRepository>();
            _service = new DeviceService(_mockRepo.Object);
        }

        [Fact]
        public async Task GetByIdAsync_Should_Return_Device()
        {
            // Arrange
            var device = new Device("Sensor", "Bosch");
            _mockRepo.Setup(r => r.GetByIdAsync(device.Id))
                     .ReturnsAsync(device);

            // Act
            var fetched = await _service.GetByIdAsync(device.Id);

            // Assert
            Assert.NotNull(fetched);
            Assert.Equal(device.Id, fetched!.Id);
        }

        [Fact]
        public async Task GetByIdAsync_InvalidId_Should_Return_Null()
        {
            // Arrange
            _mockRepo.Setup(r => r.GetByIdAsync(It.IsAny<Guid>()))
                     .ReturnsAsync((Device?)null);

            // Act
            var fetched = await _service.GetByIdAsync(Guid.NewGuid());

            // Assert
            Assert.Null(fetched);
        }

        [Fact]
        public async Task GetByIdAsync_EmptyGuid_Should_Return_Null()
        {
            // Arrange
            _mockRepo.Setup(r => r.GetByIdAsync(Guid.Empty))
                     .ReturnsAsync((Device?)null);

            // Act
            var result = await _service.GetByIdAsync(Guid.Empty);

            // Assert
            Assert.Null(result);
        }
    }
}
