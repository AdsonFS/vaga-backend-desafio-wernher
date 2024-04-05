using System.Text.RegularExpressions;
using FluentValidation;
using Wernher.Domain.Models;

namespace Wernher.API.Validation;

public class CustomerValidator : AbstractValidator<Customer>
{
    public CustomerValidator()
    {
        RuleFor(x => x.Name).NotEmpty();
        RuleFor(x => x.Email).NotEmpty().EmailAddress();
        RuleFor(x => x.Password).NotEmpty();
    }
}