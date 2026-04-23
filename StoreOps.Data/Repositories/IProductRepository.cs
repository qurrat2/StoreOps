using StoreOps.Models;

namespace StoreOps.Data.Repositories;

public interface IProductRepository : IRepository<Product>
{
    Task<Product?> GetBySkuAsync(string sku);
    Task<bool> SkuExistsAsync(string sku, int? excludeId = null);
    Task<IReadOnlyList<Product>> GetLowStockAsync();
    Task<IReadOnlyList<Product>> GetByCategoryAsync(int categoryId);
    Task<IReadOnlyList<Product>> GetByIdsAsync(IEnumerable<int> ids);
}
