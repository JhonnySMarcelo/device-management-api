using DeviceManagementApi.Application.Services;
using DeviceManagementApi.Domain.Devices.Entities;
using DeviceManagementApi.Domain.Devices.Repositories;
using Moq;
using Xunit;
using DeviceManagementApi.Tests.Helpers; // importa o helper

namespace DeviceManagementApi.Tests.Application.DeviceServiceTests
{
    public class DeviceService_GetAllAsync_Tests
    {
        private readonly Mock<IDeviceRepository> _mockRepo;
        private readonly DeviceService _service;

        public DeviceService_GetAllAsync_Tests()
        {
            _mockRepo = new Mock<IDeviceRepository>();
            _service = new DeviceService(_mockRepo.Object);
        }

        [Fact]
        public async Task GetAllAsync_Should_Return_All_Devices()
        {
            // Arrange
            var devices = new List<Device>
            {
                new Device("Sensor1", "BrandA"),
                new Device("Sensor2", "BrandB")
            };
            _mockRepo.Setup(r => r.GetAll())
                     .Returns(new TestAsyncEnumerable<Device>(devices));

            // Act
            var all = await _service.GetAllAsync();

            // Assert
            Assert.Equal(2, all.Count);
        }

        [Fact]
        public async Task GetAllAsync_WithBrandFilter_Should_Return_OnlyMatchingDevices()
        {
            // Arrange
            var devices = new List<Device>
            {
                new Device("Sensor1", "Bosch"),
                new Device("Sensor2", "Siemens")
            };
            _mockRepo.Setup(r => r.GetAll())
                     .Returns(new TestAsyncEnumerable<Device>(devices));

            // Act
            var result = await _service.GetAllAsync("Bosch");

            // Assert
            Assert.Single(result);
            Assert.Equal("Bosch", result[0].Brand, ignoreCase: true);
        }

        [Fact]
        public async Task GetAllAsync_WithStateFilter_Should_Return_OnlyMatchingDevices()
        {
            // Arrange
            var devices = new List<Device>
            {
                new Device("Sensor1", "Bosch"),
                new Device("Sensor2", "Bosch")
            };
            devices[1].ChangeState(DeviceState.InUse);

            _mockRepo.Setup(r => r.GetAll())
                     .Returns(new TestAsyncEnumerable<Device>(devices));

            // Act
            var result = await _service.GetAllAsync(state: DeviceState.InUse);

            // Assert
            Assert.Single(result);
            Assert.Equal(DeviceState.InUse, result[0].State);
        }

        [Fact]
        public async Task GetAllAsync_WhenNoDevicesExist_ShouldReturnEmptyList()
        {
            // Arrange
            _mockRepo.Setup(r => r.GetAll())
                     .Returns(new TestAsyncEnumerable<Device>(new List<Device>()));

            // Act
            var result = await _service.GetAllAsync();

            // Assert
            Assert.Empty(result);
        }

        [Fact]
        public async Task GetAllAsync_WithNonExistingBrand_ShouldReturnEmptyList()
        {
            // Arrange
            var devices = new List<Device>
            {
                new Device("Sensor1", "Bosch")
            };
            _mockRepo.Setup(r => r.GetAll())
                     .Returns(new TestAsyncEnumerable<Device>(devices));

            // Act
            var result = await _service.GetAllAsync("Siemens");

            // Assert
            Assert.Empty(result);
        }
    }
}
