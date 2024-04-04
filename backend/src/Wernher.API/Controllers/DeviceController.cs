using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using Wernher.API.DTO;

namespace Wernher.API.Controllers;
[Route("[controller]")]
[ApiController]
public class DeviceController : ControllerBase
{

    public DeviceController() { }

    [HttpPost]
    public async Task<ActionResult> PostDevice(DeviceDto device, [FromServices] IValidator<DeviceDto> validator)
    {
        var validationResult = await validator.ValidateAsync(device);
        if (!validationResult.IsValid)
            return BadRequest(validationResult.Errors);
        return Ok("Device created successfully!");
    }

    [HttpGet]
    public async Task<ActionResult> GetDevices()
    {
        return Ok();
    }

    [HttpGet("{id}")]
    public async Task<ActionResult> GetDevice(Guid id)
    {
        return Ok();
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> PutDevice(Guid id)
    {
        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteDevice(Guid id)
    {

        return NoContent();
    }

}
