using Microsoft.EntityFrameworkCore;
using Wernher.Data.Context;
using Wernher.Domain.Models;
using Wernher.Domain.Repositories;

namespace Wernher.Data.Repositories;
public class Repository<TEntity> : IDisposable, IRepository<TEntity> where TEntity : Entity
{
    private readonly DbContext _wernherContext;
    protected readonly DbSet<TEntity> _dbSet;

    public Repository(DbContext wernherContext)
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
    public async Task<TEntity> UpdateAsync(TEntity oldEntity, TEntity newEntity)
    {
        _wernherContext.Entry(oldEntity).CurrentValues.SetValues(newEntity);
        // _dbSet.Update(newEntity);
        await SaveChanges();
        return newEntity;
    }
    public async Task<int> DeleteAsync(TEntity entity)
    {
        _dbSet.Remove(entity);
        return await SaveChanges();
    }
    public virtual async Task<TEntity?> GetByIdAsync(Guid id)
        => await _dbSet
        .FindAsync(id);

    public virtual async Task<List<TEntity>> GetAllAsync()
        => await _dbSet.ToListAsync();


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