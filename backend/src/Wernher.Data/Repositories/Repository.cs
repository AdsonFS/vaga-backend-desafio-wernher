using Microsoft.EntityFrameworkCore;
using Wernher.Data.Context;
using Wernher.Domain.Models;
using Wernher.Domain.Repositories;

namespace Wernher.Data.Repositories;
public class Repository<TEntity> : IDisposable, IRepository<TEntity> where TEntity : Entity
{
    private readonly WernherContext _wernherContext;
    protected readonly DbSet<TEntity> _dbSet;

    public Repository(WernherContext wernherContext)
    {
        _wernherContext = wernherContext;
        _dbSet = _wernherContext.Set<TEntity>();
    }
    public async Task<TEntity> AddAsync(TEntity entity)
    {
        await _dbSet.AddAsync(entity);
        await SaveChanges();
        return entity;
    }
    public async Task<TEntity> UpdateAsync(TEntity entity)
    {
        _dbSet.Entry(entity).State = EntityState.Modified;
        await SaveChanges();
        return entity;
    }
    public async Task<int> DeleteAsync(TEntity entity)
    {
        _dbSet.Entry(entity).State = EntityState.Deleted;
        return await SaveChanges();
    }
    public virtual async Task<TEntity?> GetByIdAsync(Guid id)
        => await _dbSet
        .FindAsync(id);

    public async Task<List<TEntity>> GetAllAsync()
        => await _wernherContext.Set<TEntity>()
            .ToListAsync();


    public void Dispose()
    {
        _wernherContext.Dispose();
        GC.SuppressFinalize(this);
    }

    public async Task<int> SaveChanges()
    {
        return await _wernherContext.SaveChangesAsync();
    }
}