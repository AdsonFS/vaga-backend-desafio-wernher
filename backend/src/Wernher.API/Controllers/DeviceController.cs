using System.Runtime.CompilerServices;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using Wernher.API.ResponseDTO;
using Wernher.Domain.Models;
using Wernher.Domain.Repositories;
using Wernher.Domain.Telnet;

namespace Wernher.API.Controllers;
[Route("[controller]")]
[ApiController]
public class DeviceController : ControllerBase
{
    private IDeviceRepository _deviceRepository;

    public DeviceController(IDeviceRepository deviceRepository)
    {
        _deviceRepository = deviceRepository;
    }

    [HttpGet("telnet/get_rainfall_intensity")]
    public async Task<ActionResult> Telnet()
    {
        List<TelnetDataResponse> result = new();
        var devices = await _deviceRepository.GetAllAsync();
        await Task.WhenAll(devices.Select(async device =>
        {
            using var client = new Client(device.Url);
            var command = "get_rainfall_intensity";

            var telnetCommand = device.Commands
                .First(c => c.TelnetCommand.Command == command)
                .TelnetCommand;

            foreach (var parameter in telnetCommand.Parameters)
            {
                var data = await client.GetDataAsync($"{telnetCommand.GetCommandWithParameter(parameter)}");

                // filter only the float number
                data = new string(data.Where(c => char.IsDigit(c) || c == '.').ToArray());
                result.Add(new TelnetDataResponse(device.Url, command, parameter.Name, data));
            }
            await client.CloseTelnetAsync();
        }));
        return Ok(result);
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
