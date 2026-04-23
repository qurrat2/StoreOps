using StoreOps.Services.Common;

namespace StoreOps.Services.Products;

public interface IProductService
{
    Task<ProductDto?> GetByIdAsync(int id);
    Task<IReadOnlyList<ProductDto>> GetAllAsync(bool includeInactive = false);
    Task<IReadOnlyList<ProductDto>> GetByCategoryAsync(int categoryId);
    Task<IReadOnlyList<ProductDto>> GetLowStockAsync();
    Task<string> GenerateSkuAsync(int categoryId);
    Task<Result<ProductDto>> CreateAsync(CreateProductDto dto);
    Task<Result<ProductDto>> UpdateAsync(UpdateProductDto dto);
    Task<Result<bool>> DeleteAsync(int id);
}
