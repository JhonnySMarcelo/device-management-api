using DeviceManagement.Api.Domain;
using Microsoft.AspNetCore.Mvc;

namespace DeviceManagement.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class DevicesController : ControllerBase
    {

        [HttpGet]
        public ActionResult<Device> Get()
        {
            var device = new Device
            {
                Name = "Temperature Sensor",
                Brand = "Bosch",
            };

            return device;
        }
    }
}
