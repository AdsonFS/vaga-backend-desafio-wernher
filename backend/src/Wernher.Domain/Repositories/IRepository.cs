using Wernher.Domain.Models;

namespace Wernher.Domain.Repositories;
public interface IRepository<T> where T : Entity
{
    Task<T?> GetByIdAsync(Guid id);
    Task<List<T>> GetAllAsync();
    Task<T> AddAsync(T entity);
    Task<T> UpdateAsync(T oldEntity, T newEntity);
    Task<int> DeleteAsync(T entity);
}