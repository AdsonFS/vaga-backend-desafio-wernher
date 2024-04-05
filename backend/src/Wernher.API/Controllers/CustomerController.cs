
using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using Wernher.API.DTO;
using Wernher.Domain.Models;
using Wernher.Domain.Repositories;
using Wernher.Domain.Services;

namespace Wernher.API.Controllers;
[Route("[controller]")]
[ApiController]
public class CustomerController : ControllerBase
{
    private AuthenticationService _authenticationService;
    private ICustomerRepository _customerRepository;
    private IValidator<Customer> _validator;

    public CustomerController(AuthenticationService authenticationService, ICustomerRepository customerRepository, IValidator<Customer> validator)
    {
        _authenticationService = authenticationService;
        _customerRepository = customerRepository;
        _validator = validator;
    }

    /// <summary>
    /// Get a customer by id.
    /// </summary>
    /// <response code="200">Ok</response>
    /// <response code="404">Customer Not Found</response>
    /// <param name="id"></param>
    /// <returns></returns>
    [HttpGet("{id}")]
    public async Task<ActionResult<CustomerResponse>> GetCustomer(Guid id)
    {
        var customer = await _customerRepository.GetByIdAsync(id);
        if (customer == null) return NotFound();
        return Ok(new CustomerResponse(customer.Email, customer.Name));
    }

    /// <summary>
    /// Create a new customer.
    /// </summary>
    /// <response code="201">Device Created</response>
    /// <response code="400">Bad Request - Email already in use</response>
    /// <param name="customer"></param>
    /// <returns></returns>
    [HttpPost]
    public async Task<ActionResult<Customer>> PostCustomer(Customer customer)
    {
        var validationResult = await _validator.ValidateAsync(customer);
        if (!validationResult.IsValid)
            return BadRequest(validationResult.Errors);
        var customers = await _customerRepository.GetAllAsync();
        if (customers.Any(c => c.Email == customer.Email)) return BadRequest("Email already in use");

        Guid id = (await _customerRepository.AddAsync(customer)).Id;
        return CreatedAtAction(nameof(GetCustomer), new { id }, customer);
    }


    /// <summary>
    /// Login a customer.
    /// </summary>
    /// <response code="200">Ok</response>
    /// <response code="400">Bad Request - Email or Password incorrect</response>
    /// <param name="login"></param>
    /// <returns></returns>
    [HttpPost("login")]
    public async Task<ActionResult> Login([FromBody] Login login)
    {
        var customer = await _customerRepository.GetByEmailAsync(login.Email);
        if (customer == null) return BadRequest();

        var isVerified = _authenticationService.VerifyPassword(customer, login.Password);
        if (!isVerified) return BadRequest();

        var token = _authenticationService.GenerateToken(customer, false);
        return Ok(new { token, customer.Email, customer.Name });
    }

}
