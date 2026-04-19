using System.Linq.Expressions;

namespace RetailStoreAPI.Repositories;

public interface IRepository<T> where T : class
{
    Task<IEnumerable<T>> GetAllAsync(params Expression<Func<T, object?>>[] includes);
    Task<T?> GetByIdAsync(int id, params Expression<Func<T, object?>>[] includes);
    Task<bool> ExistsAsync(int id);
    Task AddAsync(T entity);
    Task UpdateAsync(T entity);
    Task DeleteAsync(T entity);
}
