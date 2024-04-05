
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

    [HttpGet("{id}")]
    public async Task<ActionResult<Customer>> GetCustomer(Guid id)
    {
        var customer = await _customerRepository.GetByIdAsync(id);
        if (customer == null) return NotFound();
        return Ok(customer);
    }


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

    [HttpPost("login")]
    public async Task<ActionResult<string>> Login([FromBody] Login login)
    {
        var customer = await _customerRepository.GetByEmailAsync(login.Email);
        if (customer == null) return BadRequest();

        var isVerified = _authenticationService.VerifyPassword(customer, login.Password);
        if (!isVerified) return BadRequest();

        var token = _authenticationService.GenerateToken(customer, false);
        return Ok(new { token, customer.Email, customer.Name });
    }

}
