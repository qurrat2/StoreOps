using StoreOps.Data.Repositories;
using StoreOps.Services.Common;

namespace StoreOps.Services.Products;

public class ProductService : IProductService
{
    private readonly IProductRepository _products;
    private readonly ICategoryRepository _categories;

    public ProductService(IProductRepository products, ICategoryRepository categories)
    {
        _products = products;
        _categories = categories;
    }

    public async Task<ProductDto?> GetByIdAsync(int id)
    {
        var entity = await _products.GetByIdAsync(id);
        if (entity is null) return null;
        var withCategory = await _products.GetBySkuAsync(entity.Sku);
        return (withCategory ?? entity).ToDto();
    }

    public async Task<IReadOnlyList<ProductDto>> GetAllAsync(bool includeInactive = false)
    {
        var entities = await _products.GetAllAsync(includeInactive);
        return entities.Select(e => e.ToDto()).ToList();
    }

    public async Task<IReadOnlyList<ProductDto>> GetByCategoryAsync(int categoryId)
    {
        var entities = await _products.GetByCategoryAsync(categoryId);
        return entities.Select(e => e.ToDto()).ToList();
    }

    public async Task<IReadOnlyList<ProductDto>> GetLowStockAsync()
    {
        var entities = await _products.GetLowStockAsync();
        return entities.Select(e => e.ToDto()).ToList();
    }

    public async Task<string> GenerateSkuAsync(int categoryId)
    {
        var category = await _categories.GetByIdAsync(categoryId);
        if (category is null) return string.Empty;

        var prefix = BuildPrefix(category.Name);
        var existing = await _products.GetByCategoryAsync(categoryId);

        var nextNumber = existing
            .Select(p => TryParseSequence(p.Sku, prefix))
            .Where(n => n.HasValue)
            .Select(n => n!.Value)
            .DefaultIfEmpty(0)
            .Max() + 1;

        return $"{prefix}-P{nextNumber}";
    }

    public async Task<Result<ProductDto>> CreateAsync(CreateProductDto dto)
    {
        var validation = Validate(dto.Sku, dto.Name, dto.Price, dto.CostPrice);
        if (validation is not null) return Result<ProductDto>.Failure(validation);

        if (!await _categories.ExistsAsync(dto.CategoryId))
        {
            return Result<ProductDto>.Failure("Selected category does not exist.");
        }

        var normalizedSku = dto.Sku.Trim().ToUpperInvariant();
        if (await _products.SkuExistsAsync(normalizedSku))
        {
            return Result<ProductDto>.Failure($"SKU '{normalizedSku}' already exists.");
        }

        var entity = dto.ToEntity();
        var created = await _products.AddAsync(entity);
        var withCategory = await _products.GetBySkuAsync(created.Sku);
        return Result<ProductDto>.Success((withCategory ?? created).ToDto());
    }

    public async Task<Result<ProductDto>> UpdateAsync(UpdateProductDto dto)
    {
        var validation = Validate(dto.Sku, dto.Name, dto.Price, dto.CostPrice);
        if (validation is not null) return Result<ProductDto>.Failure(validation);

        var entity = await _products.GetByIdAsync(dto.Id);
        if (entity is null)
        {
            return Result<ProductDto>.Failure("Product not found.");
        }

        if (!await _categories.ExistsAsync(dto.CategoryId))
        {
            return Result<ProductDto>.Failure("Selected category does not exist.");
        }

        var normalizedSku = dto.Sku.Trim().ToUpperInvariant();
        if (await _products.SkuExistsAsync(normalizedSku, excludeId: dto.Id))
        {
            return Result<ProductDto>.Failure($"SKU '{normalizedSku}' already exists.");
        }

        entity.UpdateEntity(dto);
        var updated = await _products.UpdateAsync(entity);
        var withCategory = await _products.GetBySkuAsync(updated.Sku);
        return Result<ProductDto>.Success((withCategory ?? updated).ToDto());
    }

    public async Task<Result<bool>> DeleteAsync(int id)
    {
        if (!await _products.ExistsAsync(id))
        {
            return Result<bool>.Failure("Product not found.");
        }

        await _products.DeleteAsync(id);
        return Result<bool>.Success(true);
    }

    private static string? Validate(string sku, string name, decimal price, decimal costPrice)
    {
        if (string.IsNullOrWhiteSpace(sku)) return "SKU is required.";
        if (sku.Length > 50) return "SKU must be 50 characters or fewer.";
        if (string.IsNullOrWhiteSpace(name)) return "Name is required.";
        if (name.Length > 200) return "Name must be 200 characters or fewer.";
        if (price <= 0) return "Price must be greater than zero.";
        if (costPrice <= 0) return "Cost price must be greater than zero.";
        return null;
    }

    private static string BuildPrefix(string categoryName)
    {
        var firstWord = categoryName.Trim().Split(' ', StringSplitOptions.RemoveEmptyEntries).FirstOrDefault() ?? string.Empty;
        var letters = new string(firstWord.Where(char.IsLetter).ToArray()).ToUpperInvariant();
        if (string.IsNullOrEmpty(letters)) return "CAT";
        return letters.Length >= 4 ? letters[..4] : letters;
    }

    private static int? TryParseSequence(string sku, string prefix)
    {
        var marker = $"{prefix}-P";
        if (!sku.StartsWith(marker, StringComparison.OrdinalIgnoreCase)) return null;
        var tail = sku[marker.Length..];
        return int.TryParse(tail, out var n) ? n : null;
    }
}
