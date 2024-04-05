using Wernher.Domain.Models;

namespace Wernher.Domain.Repositories;

public interface ICustomerRepository : IRepository<Customer>
{
    Task<Customer?> GetByEmailAsync(string email);
}