using System.Linq.Expressions;
using API.Implementations.Repository.Entities;

namespace API.Abstractions;

public interface IRepository<T> where T : GenericEntity
{
    Task<T?> GetByIdAsync(int id);
    Task<IEnumerable<T>> GetAllAsync();
    Task<T> AddAsync(T entity);
    Task UpdateAsync(T entity);
    Task DeleteAsync(T entity);
    IQueryable<T> Get(Expression<Func<T, bool>>? filter = null, string? includeProperties = null);
    Task AddRangeAsync(IEnumerable<T> entities);
}