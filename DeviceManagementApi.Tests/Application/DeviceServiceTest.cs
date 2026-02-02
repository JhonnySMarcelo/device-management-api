using DeviceManagementApi.Application;
using DeviceManagementApi.Domain;
using DeviceManagementApi.Infrastructure;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace DeviceManagementApi.Tests.Application
{
    public class DeviceServiceTest
    {
        private DeviceManagementDbContext GetDbContext()
        {
            var options = new DbContextOptionsBuilder<DeviceManagementDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

            return new DeviceManagementDbContext(options);
        }

        [Fact]
        public async Task CreateAsync_Should_Create_Device()
        {
            var context = GetDbContext();
            var service = new DeviceService(context);

            var device = await service.CreateAsync("Sensor", "Bosch");

            Assert.NotNull(device);
            Assert.Equal("Sensor", device.Name);
            Assert.Equal("Bosch", device.Brand);
            Assert.Equal(DeviceState.Available, device.State);
            Assert.NotEqual(Guid.Empty, device.Id);
        }

    }
}
