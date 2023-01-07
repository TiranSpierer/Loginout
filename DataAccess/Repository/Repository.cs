using DataAccess.Context;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DataAccess.Repository;

public class Repository<T> : IRepository<T> where T : class
{
    private readonly EnvueDbContextFactory _dbContextFactory;

    public Repository(EnvueDbContextFactory dbContextFactory)
	{
        _dbContextFactory = dbContextFactory;
    }

    #region Implementation of IRepository<T>

    public virtual async Task<bool> Create(T entity)
    {
        using EnvueDbContext context = _dbContextFactory.CreateDbContext();
        var isEntityCreated = true;
        try
        {
            await context.Set<T>().AddAsync(entity);
            await context.SaveChangesAsync();
        }
        catch
        {
            isEntityCreated = false;
        }

        return isEntityCreated;
    }

    public virtual async Task<T?> GetById(params object[] id)
    {
        using EnvueDbContext context = _dbContextFactory.CreateDbContext();
        T? entity = null;

        if (IsIdValid(id))
        {
            entity = await context.Set<T>().FindAsync(id);
        }
        return entity;
    }

    public virtual async Task<IEnumerable<T>> GetAll()
    {
        using EnvueDbContext context = _dbContextFactory.CreateDbContext();
        return await context.Set<T>().ToListAsync();
    }

    public virtual async Task Update(object id, T updatedEntity)
    {
        using EnvueDbContext context = _dbContextFactory.CreateDbContext();
        var entity = await GetById(id);

        if (entity != null)
        {
            try
            {
                //updatedEntity.CopyValuesTo(entity);
                context.Set<T>().Update(entity);
                await context.SaveChangesAsync();
            }
            catch
            {
                await Delete(entity);
                await Create(updatedEntity);
            }
        }
    }

    public virtual async Task Delete(object id)
    {
        using EnvueDbContext context = _dbContextFactory.CreateDbContext();
        var entity = await GetById(id);
        if (entity != null)
        {
            context.Set<T>().Remove(entity);
            await context.SaveChangesAsync();
        }
    }

#endregion

    #region Private Methods

    private bool IsIdValid(object id)
    {
        return id.GetType() != typeof(string) || id is string s && string.IsNullOrEmpty(s) == false;
    }

    #endregion

}
