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

        /// <summary>
        /// Creates a new device.
        /// </summary>
        /// <remarks>
        /// Business rules:
        /// - Name and Brand are required
        /// - Device is created with state 'Available'
        /// - Creation time is automatically assigned
        /// </remarks>
        /// <response code="201">Device successfully created</response>
        /// <response code="400">Invalid input data</response>
        [HttpPost]
        [ProducesResponseType(typeof(Device), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ValidationProblemDetails), StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<Device>> Create([FromBody] CreateDeviceRequest request)
        {
            var device = await _deviceService.CreateAsync(request.Name, request.Brand);

            return CreatedAtAction(nameof(GetById), new { id = device.Id }, device);
        }

        /// <summary>
        /// Retrieves a device by its identifier.
        /// </summary>
        /// <response code="200">Device found</response>
        /// <response code="404">Device not found</response>
        [HttpGet("{id:guid}")]
        [ProducesResponseType(typeof(Device), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<Device>> GetById(Guid id)
        {
            var device = await _deviceService.GetByIdAsync(id);

            if (device == null) return NotFound();

            return Ok(device);
        }

        /// <summary>
        /// Retrieves all devices.
        /// </summary>
        /// <response code="200">Devices found</response>
        [HttpGet]
        [ProducesResponseType(typeof(List<Device>), StatusCodes.Status200OK)]
        public async Task<ActionResult<List<Device>>> GetAll()
        {
            var devices = await _deviceService.GetAllAsync();
            return Ok(devices);
        }

        /// <summary>
        /// Retrieves devices filtered by brand.
        /// </summary>
        /// <param name="brand">Brand name to filter devices</param>
        /// <response code="200">Devices found</response>
        [HttpGet("brand/{brand}")]
        [ProducesResponseType(typeof(List<Device>), StatusCodes.Status200OK)]
        public async Task<ActionResult<List<Device>>> GetAllByBrand(string brand)
        {
            var devices = await _deviceService.GetAllByBrandAsync(brand);
            return Ok(devices);
        }

        /// <summary>
        /// Retrieves devices filtered by state.
        /// </summary>
        /// <param name="state">Device state to filter</param>
        /// <response code="200">Devices found</response>
        [HttpGet("state/{state}")]
        [ProducesResponseType(typeof(List<Device>), StatusCodes.Status200OK)]
        public async Task<ActionResult<List<Device>>> GetAllByState(DeviceState state)
        {
            var devices = await _deviceService.GetAllByStateAsync(state);
            return Ok(devices);
        }

        /// <summary>
        /// Updates an existing device.
        /// </summary>
        /// <remarks>
        /// Business rules:
        /// - Creation time cannot be updated
        /// - Name and Brand cannot be changed if device is in use
        /// </remarks>
        /// <response code="200">Device successfully updated</response>
        /// <response code="400">Invalid input data</response>
        /// <response code="404">Device not found</response>
        /// <response code="409">Business rule violation</response>
        [HttpPatch("{id:guid}")]
        [ProducesResponseType(typeof(Device), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ValidationProblemDetails), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status409Conflict)]
        public async Task<ActionResult<Device>> Update(Guid id, [FromBody] UpdateDeviceRequest request)
        {
            try
            {
                var updatedDevice = await _deviceService.UpdateAsync(id, request);

                if (updatedDevice == null) return NotFound();

                return Ok(updatedDevice);
            }
            catch (InvalidOperationException ex)
            {
                return Conflict(new ProblemDetails
                {
                    Title = "Business rule violation",
                    Detail = ex.Message,
                    Status = StatusCodes.Status409Conflict
                });
            }
        }

        /// <summary>
        /// Deletes a device.
        /// </summary>
        /// <remarks>
        /// Business rules:
        /// - Devices in use cannot be deleted
        /// </remarks>
        /// <response code="204">Device successfully deleted</response>
        /// <response code="404">Device not found</response>
        /// <response code="409">Device is in use</response>
        [HttpDelete("{id:guid}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status409Conflict)]
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
                return Conflict(new ProblemDetails
                {
                    Title = "Business rule violation",
                    Detail = ex.Message,
                    Status = StatusCodes.Status409Conflict
                });
            }
        }
    }
}
