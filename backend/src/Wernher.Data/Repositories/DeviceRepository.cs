using Microsoft.EntityFrameworkCore;
using Wernher.Data.Context;
using Wernher.Domain.Models;

namespace Wernher.Data.Repositories;

public class DeviceRepository : Repository<Device>
{
    public DeviceRepository(WernherContext wernherContext) : base(wernherContext) { }


    public override async Task<Device?> GetByIdAsync(Guid id)
        => await _dbSet
        .Include(x => x.Commands)
        .ThenInclude(x => x.TelnetCommand)
        .ThenInclude(x => x.Parameters)
        .FirstOrDefaultAsync(x => x.Id == id);
}