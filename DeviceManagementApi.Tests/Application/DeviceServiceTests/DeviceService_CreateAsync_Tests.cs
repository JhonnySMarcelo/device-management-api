using DeviceManagementApi.Application.Services;
using DeviceManagementApi.Domain.Devices.Entities;
using DeviceManagementApi.Domain.Devices.Repositories;
using Moq;
using Xunit;

namespace DeviceManagementApi.Tests.Application.DeviceServiceTests
{
    public class DeviceService_CreateAsync_Tests
    {
        private readonly Mock<IDeviceRepository> _mockRepo;
        private readonly DeviceService _service;

        public DeviceService_CreateAsync_Tests()
        {
            _mockRepo = new Mock<IDeviceRepository>();
            _service = new DeviceService(_mockRepo.Object);
        }

        [Fact]
        public async Task CreateAsync_Should_Create_Device()
        {
            //Arrange
            _mockRepo.Setup(r => r.AddAsync(It.IsAny<Device>()))
                    .Returns(Task.CompletedTask);
            _mockRepo.Setup(r => r.SaveChangesAsync())
                     .Returns(Task.CompletedTask);

            //Act
            var device = await _service.CreateAsync("Sensor", "Bosch");

            //Assert
            Assert.NotNull(device);
            Assert.Equal("Sensor", device.Name);
            Assert.Equal("Bosch", device.Brand);
            Assert.Equal(DeviceState.Available, device.State);
            Assert.NotEqual(Guid.Empty, device.Id);
        }

        [Fact]
        public async Task CreateAsync_Should_Set_State_To_Available()
        {
            // Arrange
            _mockRepo.Setup(r => r.AddAsync(It.IsAny<Device>()))
                     .Returns(Task.CompletedTask);
            _mockRepo.Setup(r => r.SaveChangesAsync())
                     .Returns(Task.CompletedTask);

            // Act
            var device = await _service.CreateAsync("Sensor", "Bosch");

            // Assert
            Assert.Equal(DeviceState.Available, device.State);
        }

        [Fact]
        public async Task CreateAsync_Should_Set_Id_NotEmpty()
        {
            // Arrange
            _mockRepo.Setup(r => r.AddAsync(It.IsAny<Device>()))
                     .Returns(Task.CompletedTask);
            _mockRepo.Setup(r => r.SaveChangesAsync())
                     .Returns(Task.CompletedTask);

            // Act
            var device = await _service.CreateAsync("Sensor", "Bosch");

            // Assert
            Assert.NotEqual(Guid.Empty, device.Id);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("   ")]
        public async Task CreateAsync_Should_Throw_When_Name_Invalid(string? invalidName)
        {
            // Arrange
            // Act & Assert
            await Assert.ThrowsAsync<InvalidOperationException>(
                () => _service.CreateAsync(invalidName!, "Brand")
            );
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("   ")]
        public async Task CreateAsync_Should_Throw_When_Brand_Invalid(string? invalidBrand)
        {
            // Arrange
            // Act & Assert
            await Assert.ThrowsAsync<InvalidOperationException>(
                () => _service.CreateAsync("Name", invalidBrand!)
            );
        }
    }

}
