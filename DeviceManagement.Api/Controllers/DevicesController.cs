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

        [HttpPost]
        public async Task<ActionResult<Device>> Create(CreateDeviceRequest request)
        {
            var device = await _deviceService.CreateAsync(
                request.Name, request.Brand
                );

            return CreatedAtAction(
                nameof(GetById),
                new { id = device.Id },
                device
                );
        }


        [HttpGet("{id:guid}")]
        public async Task<ActionResult<Device>> GetById(Guid id)
        {
            var device = await _deviceService.GetByIdAsync(id);

            if (device == null) return NotFound();

            return Ok(device);
        }

        [HttpGet]
        public async Task<ActionResult<List<Device>>> GetAll()
        {
            var devices = await _deviceService.GetAllAsync();

            if (devices.Count == 0) return NotFound();

            return Ok(devices);
        }

        [HttpGet("brand/{brand}")]
        public async Task<ActionResult<List<Device>>> GetAllByBrand(string brand)
        {
            var devices = await _deviceService.GetAllByBrandAsync(brand);

            if (devices.Count == 0) return NotFound();

            return Ok(devices);
        }

        [HttpGet("state/{state}")]
        public async Task<ActionResult<List<Device>>> GetAllByState(DeviceState state)
        {
            var devices = await _deviceService.GetAllByStateAsync(state);

            if (devices.Count == 0) return NotFound();

            return Ok(devices);
        }

        [HttpPatch("{id:guid}")]
        public async Task<ActionResult<Device>> Update(Guid id, UpdateDeviceRequest request)
        {
            try
            {
                var updatedDevice = await _deviceService.UpdateAsync(id, request);

                if (updatedDevice == null) return NotFound();

                return Ok(updatedDevice);
            }
            catch (InvalidOperationException ex)
            {
                return Conflict(ex.Message);
            }
        }

        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            try
            {
                var deleted = await _deviceService.DeleteAsync(id);

                if (deleted == null) return NotFound();

                return NoContent();
            }
            catch (InvalidOperationException ex)
            {
                return Conflict(ex.Message);
            }
        }

    }
}
