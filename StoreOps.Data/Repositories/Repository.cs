using Microsoft.EntityFrameworkCore;
using StoreOps.Models;

namespace StoreOps.Data.Repositories;

public class Repository<T> : IRepository<T> where T : BaseEntity
{
    protected readonly IDbContextFactory<AppDbContext> Factory;

    public Repository(IDbContextFactory<AppDbContext> factory)
    {
        Factory = factory;
    }

    public virtual async Task<T?> GetByIdAsync(int id)
    {
        using var context = await Factory.CreateDbContextAsync();
        return await context.Set<T>().FindAsync(id);
    }

    public virtual async Task<IReadOnlyList<T>> GetAllAsync(bool includeInactive = false)
    {
        using var context = await Factory.CreateDbContextAsync();
        var query = context.Set<T>().AsQueryable();

        if (!includeInactive)
        {
            query = query.Where(e => e.IsActive);
        }

        return await query.AsNoTracking().ToListAsync();
    }

    public virtual async Task<T> AddAsync(T entity)
    {
        using var context = await Factory.CreateDbContextAsync();
        context.Set<T>().Add(entity);
        await context.SaveChangesAsync();
        return entity;
    }

    public virtual async Task<T> UpdateAsync(T entity)
    {
        using var context = await Factory.CreateDbContextAsync();
        entity.UpdatedAt = DateTime.UtcNow;
        context.Set<T>().Update(entity);
        await context.SaveChangesAsync();
        return entity;
    }

    public virtual async Task DeleteAsync(int id)
    {
        using var context = await Factory.CreateDbContextAsync();
        var entity = await context.Set<T>().FindAsync(id);
        if (entity is null) return;

        entity.IsActive = false;
        entity.UpdatedAt = DateTime.UtcNow;
        await context.SaveChangesAsync();
    }

    public virtual async Task<bool> ExistsAsync(int id)
    {
        using var context = await Factory.CreateDbContextAsync();
        return await context.Set<T>().AnyAsync(e => e.Id == id);
    }
}
