using Microsoft.EntityFrameworkCore;
using Wernher.Data.Context;
using Wernher.Domain.Models;
using Wernher.Domain.Repositories;

namespace Wernher.Data.Repositories;

public class CustomerRepository : Repository<Customer>, ICustomerRepository
{
    public CustomerRepository(WernherContext wernherContext) : base(wernherContext) { }

    public async Task<Customer?> GetByEmailAsync(string email)
        => await _dbSet.FirstOrDefaultAsync(x => x.Email == email);
}
