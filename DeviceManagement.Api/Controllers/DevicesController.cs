using DeviceManagementApi.Application;
using DeviceManagementApi.Application.DTOs;
using DeviceManagementApi.Domain;
using Microsoft.AspNetCore.Mvc;

namespace DeviceManagementApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class DevicesController : ControllerBase
    {
        private readonly DeviceService _deviceService;

        public DevicesController(DeviceService deviceService)
        {
            _deviceService = deviceService;
        }

        [HttpGet]
        public ActionResult<Device> Get()
        {
            string name = "Temperature Sensor";
            string brand = "Bosch";

            var device = new Device(name, brand);

            return device;
        }

        [HttpPost]
        public async Task<ActionResult<Device>> Create(CreateDeviceRequest request)
        { 
            var device = await _deviceService.CreateAsync(
                request.Name, request.Brand
                );

            return CreatedAtAction(
                nameof(Get),
                new { id = device.Id },
                device
                );
        }
    }
}
