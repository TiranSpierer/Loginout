using DataAccess.Context;
using DataAccess.DataModels;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Linq;
using System.Threading.Tasks;

namespace DataAccess.Repository;

public class Repository<T> : IRepository<T> where T : class, IEntity<T>
{
    private readonly EnvueDbContextFactory _dbContextFactory;

    public Repository(EnvueDbContextFactory dbContextFactory)
	{
        _dbContextFactory = dbContextFactory;
    }

    #region Implementation of IRepository<T>

    public async Task<bool> CreateAsync(T entity)
    {
        await using var context = _dbContextFactory.CreateDbContext();
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

    public async Task<T?> GetByIdAsync(params object[] id)
    {
        await using var context = _dbContextFactory.CreateDbContext();
        T? entity = null;

        if (IsIdValid(id))
        {
            entity = await context.Set<T>().FindAsync(id);
        }
        return entity;
    }

    public async Task<IEnumerable<T>> GetAllAsync()
    {
        await using var context = _dbContextFactory.CreateDbContext();
        return await context.Set<T>().ToListAsync();
    }

    public async Task<IEnumerable<T>> GetAllIncludingPropertiesAsync(params Expression<Func<T, object>>[] includeProperties)
    {
        await using var context = _dbContextFactory.CreateDbContext();
        IQueryable<T> query = context.Set<T>();
        foreach (var includeProperty in includeProperties)
        {
            query = query.Include(includeProperty);
        }
        return await query.ToListAsync();
    }


    public async Task<bool> UpdateAsync(object id, T updatedEntity)
    {
        await using var context   = _dbContextFactory.CreateDbContext();
        var             entity    = await GetByIdAsync(id);
        var             isUpdated = true;

        if (entity != null)
        {
            try
            {
                updatedEntity.CopyValuesTo(entity);
                context.Set<T>().Update(entity);
                await context.SaveChangesAsync();
            }
            catch
            {
                isUpdated = false;
            }
        }

        return isUpdated;
    }

    public async Task<bool> DeleteAsync(object id)
    {
        await using var context   = _dbContextFactory.CreateDbContext();
        var             entity    = await GetByIdAsync(id);
        var             isDeleted = false;

        if (entity != null)
        {
            try
            {
                context.Set<T>().Remove(entity);
                await context.SaveChangesAsync();
                isDeleted = true;
            }
            catch
            {
                isDeleted = false;
            }
        }

        return isDeleted;
    }

#endregion

    #region Private Methods

    private bool IsIdValid(object id)
    {
        return id.GetType() != typeof(string) || id is string s && string.IsNullOrEmpty(s) == false;
    }

    #endregion

}
