using DataAccess.DataModels;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace DataAccess.Repository;

public interface IRepository<T> where T : class, IEntity<T>
{
    Task<bool> CreateAsync(T entity);

    Task<T?> GetByIdAsync(params object[] id);

    Task<T?> GetByIdIncludingPropertiesAsync(Expression<Func<T, bool>> idPredicate, params Expression<Func<T, object>>[] includeProperties);

    Task<IEnumerable<T>> GetAllAsync();

    Task<IEnumerable<T>> GetAllIncludingPropertiesAsync(params Expression<Func<T, object>>[] includeProperties);

    Task<bool> UpdateAsync(object id, T updatedEntity);

    Task<bool> DeleteAsync(object id);
}