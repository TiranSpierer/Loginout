using DataAccess.DataModels;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace DataAccess.Repository;

public interface IRepository<T> where T : class, IEntity<T>
{
    Task<bool> Create(T entity);
    Task<T?> GetById(params object[] id);
    Task<IEnumerable<T>> GetAll();
    Task<IEnumerable<T>> GetAllIncluding(params Expression<Func<T, object>>[] includeProperties);
    Task<bool> Update(object id, T updatedEntity);
    Task Delete(object id);
}