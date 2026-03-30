using System.Text.Json.Serialization;

namespace DeviceManagementApi.Domain.Devices.Entities
{
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum DeviceState
    {
        Available = 0,
        InUse = 1, 
        Inactive = 2
    }
}
