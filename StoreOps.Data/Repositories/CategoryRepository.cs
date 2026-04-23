using Microsoft.EntityFrameworkCore;
using StoreOps.Models;

namespace StoreOps.Data.Repositories;

public class CategoryRepository : Repository<Category>, ICategoryRepository
{
    public CategoryRepository(IDbContextFactory<AppDbContext> factory) : base(factory)
    {
    }

    public async Task<Category?> GetByNameAsync(string name)
    {
        using var context = await Factory.CreateDbContextAsync();
        return await context.Categories
            .AsNoTracking()
            .FirstOrDefaultAsync(c => c.Name == name);
    }

    public async Task<bool> NameExistsAsync(string name, int? excludeId = null)
    {
        using var context = await Factory.CreateDbContextAsync();
        var query = context.Categories.Where(c => c.Name == name);

        if (excludeId.HasValue)
        {
            query = query.Where(c => c.Id != excludeId.Value);
        }

        return await query.AnyAsync();
    }
}
