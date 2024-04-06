
using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using Wernher.API.DTO;
using Wernher.Domain.Models;
using Wernher.Domain.Repositories;
using Wernher.Domain.Services;

namespace Wernher.API.Controllers;
[Route("")]
[ApiController]
public class PingController : ControllerBase
{

    /// <summary>
    /// Ping the server.
    /// </summary>
    /// <response code="200">Ok</response>
    /// <returns></returns>
    [HttpGet]
    public ActionResult Ping() => Ok();
}
