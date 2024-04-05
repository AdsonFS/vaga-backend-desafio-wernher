using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using Wernher.Domain.Models;
using Wernher.Domain.Repositories;

namespace Wernher.API.Controllers;
[Route("[controller]")]
[ApiController]
public class DeviceController : ControllerBase
{
    private IRepository<Device> _deviceRepository;

    public DeviceController(IRepository<Device> deviceRepository)
    {
        _deviceRepository = deviceRepository;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<Device>>> GetDevices()
    {
        return Ok(await _deviceRepository.GetAllAsync());
    }
    [HttpPost]
    public async Task<ActionResult<Device>> PostDevice(Device device, [FromServices] IValidator<Device> validator)
    {
        var validationResult = await validator.ValidateAsync(device);
        if (!validationResult.IsValid)
            return BadRequest(validationResult.Errors);

        Guid id = (await _deviceRepository.AddAsync(device)).Id;
        return CreatedAtAction(nameof(GetDevice), new { id }, device);

    }


    [HttpGet("{id}")]
    public async Task<ActionResult<Device>> GetDevice(Guid id)
    {
        var device = await _deviceRepository.GetByIdAsync(id);
        if (device == null) return NotFound();
        return Ok(device);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> PutDevice(Guid id, [FromBody] Device newDevice, [FromServices] IValidator<Device> validator)
    {
        var validationResult = await validator.ValidateAsync(newDevice);
        if (!validationResult.IsValid)
            return BadRequest(validationResult.Errors);

        var device = await _deviceRepository.GetByIdAsync(id);
        if (device == null) return BadRequest();

        await _deviceRepository.UpdateAsync(device, newDevice);
        return Ok();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteDevice(Guid id)
    {
        var device = await _deviceRepository.GetByIdAsync(id);
        if (device == null) return BadRequest();

        await _deviceRepository.DeleteAsync(device);
        return Ok();
    }

}
