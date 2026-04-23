using Microsoft.EntityFrameworkCore;
using StoreOps.Models;

namespace StoreOps.Data.Repositories;

public class ProductRepository : Repository<Product>, IProductRepository
{
    public ProductRepository(IDbContextFactory<AppDbContext> factory) : base(factory)
    {
    }

    public async Task<Product?> GetBySkuAsync(string sku)
    {
        using var context = await Factory.CreateDbContextAsync();
        return await context.Products
            .Include(p => p.Category)
            .AsNoTracking()
            .FirstOrDefaultAsync(p => p.Sku == sku);
    }

    public async Task<bool> SkuExistsAsync(string sku, int? excludeId = null)
    {
        using var context = await Factory.CreateDbContextAsync();
        var query = context.Products.Where(p => p.Sku == sku);

        if (excludeId.HasValue)
        {
            query = query.Where(p => p.Id != excludeId.Value);
        }

        return await query.AnyAsync();
    }

    public async Task<IReadOnlyList<Product>> GetLowStockAsync()
    {
        using var context = await Factory.CreateDbContextAsync();
        return await context.Products
            .Include(p => p.Category)
            .Where(p => p.IsActive && p.StockQuantity <= p.ReorderLevel)
            .AsNoTracking()
            .ToListAsync();
    }

    public async Task<IReadOnlyList<Product>> GetByCategoryAsync(int categoryId)
    {
        using var context = await Factory.CreateDbContextAsync();
        return await context.Products
            .Where(p => p.IsActive && p.CategoryId == categoryId)
            .AsNoTracking()
            .ToListAsync();
    }

    public override async Task<IReadOnlyList<Product>> GetAllAsync(bool includeInactive =false)
    {
        using var context = await Factory.CreateDbContextAsync();
        var query = context.Products.Include(p => p.Category).AsNoTracking();
        if (!includeInactive) query= query.Where(p => p.IsActive);
        return await query.ToListAsync();
    }

    public async Task<IReadOnlyList<Product>> GetByIdsAsync(IEnumerable<int> ids)
    {
        var idList = ids.Distinct().ToList();
        using var context = await Factory.CreateDbContextAsync();
        return await context.Products
            .Where(p => idList.Contains(p.Id))
            .Include(p => p.Category)
            .AsNoTracking()
            .ToListAsync();
    }
}
