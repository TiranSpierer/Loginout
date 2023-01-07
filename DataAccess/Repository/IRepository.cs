using System.Collections.Generic;
using System.Threading.Tasks;

namespace DataAccess.Repository;

public interface IRepository<T> where T : class
{
    Task<bool> Create(T entity);
    Task<T?> GetById(params object[] id);
    Task<IEnumerable<T>> GetAll();
    Task Update(object id, T updatedEntity);
    Task Delete(object id);
}
