using DeviceManagementApi.Domain;
using Xunit;

namespace DeviceManagementApi.Tests.Domain.DeviceTests
{
    public class Device_ChangeState_Tests
    {
        [Fact]
        public void ChangeState_Should_Update_State()
        {
            var device = new Device("Name", "Brand");

            device.ChangeState(DeviceState.InUse);

            Assert.Equal(DeviceState.InUse, device.State);
        }

        [Fact]
        public void ChangeState_Same_State_Should_Not_Change()
        {
            var device = new Device("Name", "Brand");

            device.ChangeState(DeviceState.Available);

            Assert.Equal(DeviceState.Available, device.State);
        }

        [Fact]
        public void ChangeState_Should_Allow_Multiple_Transitions()
        {
            var device = new Device("Name", "Brand");

            device.ChangeState(DeviceState.Inactive);
            Assert.Equal(DeviceState.Inactive, device.State);

            device.ChangeState(DeviceState.InUse);
            Assert.Equal(DeviceState.InUse, device.State);
        }

    }
}
