using Microsoft.EntityFrameworkCore;
using Wernher.Data.Context;
using Wernher.Domain.Models;
using Wernher.Domain.Repositories;

namespace Wernher.Data.Repositories;

public class DeviceRepository : Repository<Device>, IDeviceRepository
{
    public DeviceRepository(WernherContext wernherContext) : base(wernherContext) { }

    public virtual async Task<List<Device>> GetAllAsync()
        => await _dbSet
            .Include(x => x.Commands)
            .ThenInclude(x => x.TelnetCommand)
            .ThenInclude(x => x.Parameters)
            .ToListAsync();

    public override async Task<Device?> GetByIdAsync(Guid id)
        => await _dbSet
        .Include(x => x.Commands)
        .ThenInclude(x => x.TelnetCommand)
        .ThenInclude(x => x.Parameters)
        .FirstOrDefaultAsync(x => x.Id == id);

    public override async Task<Device> UpdateAsync(Device oldEntity, Device newEntity)
    {
        _dbSet.Remove(oldEntity);
        oldEntity.Update(newEntity.Description, newEntity.Manufacturer, newEntity.Url, newEntity.Commands);
        _dbSet.Update(oldEntity);
        await SaveChanges();
        return newEntity;
    }


}
