using System.Runtime.CompilerServices;
using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Wernher.API.DTO;
using Wernher.Domain.Models;
using Wernher.Domain.Repositories;
using Wernher.Domain.Telnet;

namespace Wernher.API.Controllers;
[Route("[controller]")]
[ApiController]
[Authorize]
public class DeviceController : ControllerBase
{
    private IDeviceRepository _deviceRepository;
    public DeviceController(IDeviceRepository deviceRepository)
    {
        _deviceRepository = deviceRepository;
    }
    private string GetCustomerId()
        => User.Claims
            .Where(c => c.Type == "CustomerId")
            .Select(x => x.Value).FirstOrDefault()!;

    /// <summary>
    /// Get the rainfall intensity from all devices.
    /// </summary>
    /// <response code="200">Ok</response>
    /// <response code="401">Unauthorized</response>
    /// <returns></returns>
    [HttpGet("telnet/get_rainfall_intensity")]
    public async Task<ActionResult<IEnumerable<TelnetDataResponse>>> Telnet()
    {
        List<TelnetDataResponse> result = new();
        var devices = await _deviceRepository.GetAllAsync();
        await Task.WhenAll(devices.Select(async device =>
        {
            try
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
            }
            catch (Exception)
            {
                result.Add(new TelnetDataResponse(device.Url, "Error", "Error", "Connection Error"));
            }
        }));
        return Ok(result);
    }

    /// <summary>
    /// Get all devices.
    /// </summary>
    /// <response code="200">Ok</response>
    /// <response code="401">Unauthorized</response>
    /// <returns></returns>
    [HttpGet]
    public async Task<ActionResult<IEnumerable<Device>>> GetDevices()
        => Ok(await _deviceRepository.GetAllAsync());


    /// <summary>
    /// Create a new device.
    /// </summary>
    /// <response code="201">Device Created</response>
    /// <response code="400">Bad Request</response>
    /// <response code="401">Unauthorized</response>
    /// <param name="device"></param>
    /// <param name="validator"></param>
    /// <returns></returns>
    [HttpPost]
    public async Task<ActionResult<Device>> PostDevice(Device device, [FromServices] IValidator<Device> validator)
    {
        var validationResult = await validator.ValidateAsync(device);
        if (!validationResult.IsValid)
            return BadRequest(validationResult.Errors);
        device.CustomerId = Guid.Parse(GetCustomerId());
        Guid id = (await _deviceRepository.AddAsync(device)).Id;
        return CreatedAtAction(nameof(GetDevice), new { id }, device);
    }


    /// <summary>
    /// Get a device by id.
    /// </summary>
    /// <response code="200">Ok</response>
    /// <response code="401">Unauthorized</response>
    /// <response code="404">Device Not Found</response>
    /// <param name="id"></param>
    /// <returns></returns>
    [HttpGet("{id}")]
    public async Task<ActionResult<Device>> GetDevice(Guid id)
    {
        var device = await _deviceRepository.GetByIdAsync(id);
        if (device == null) return NotFound();
        return Ok(device);
    }

    /// <summary>
    /// Update a device.
    /// </summary>
    /// <response code="200">Ok</response>
    /// <response code="400">Bad Request</response>
    /// <response code="401">Unauthorized</response>
    /// <response code="404">Device Not Found</response>
    /// <param name="id"></param>
    /// <param name="newDevice"></param>
    /// <param name="validator"></param>
    /// <returns></returns>
    [HttpPut("{id}")]
    public async Task<ActionResult<Device>> PutDevice(Guid id, [FromBody] Device newDevice, [FromServices] IValidator<Device> validator)
    {
        var validationResult = await validator.ValidateAsync(newDevice);
        if (!validationResult.IsValid)
            return BadRequest(validationResult.Errors);

        var device = await _deviceRepository.GetByIdAsync(id);
        if (device == null) return NotFound();

        if (device.CustomerId.ToString() != GetCustomerId()) return Unauthorized();

        await _deviceRepository.UpdateAsync(device, newDevice);
        return Ok(device);
    }

    /// <summary>
    /// Delete a device.
    /// </summary>
    /// <response code="200">Ok</response>
    /// <response code="401">Unauthorized</response>
    /// <response code="404">Device Not Found</response>
    /// <param name="id"></param>
    /// <returns></returns>
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteDevice(Guid id)
    {
        var device = await _deviceRepository.GetByIdAsync(id);
        if (device == null) return NotFound();

        if (device.CustomerId.ToString() != GetCustomerId()) return Unauthorized();

        await _deviceRepository.DeleteAsync(device);
        return Ok();
    }

}
