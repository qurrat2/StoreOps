using System;
using System.Collections.Generic;
using System.Text;

namespace StoreOps.Services.Products
{
    public record ProductDto(
        int Id, string Sku, string Name, string? Description, decimal Price,
        decimal CostPrice, int StockQuantity, int ReorderLevel, string? ImageUrl, int CategoryId, string CategoryName,
        DateTime CreatedAt, DateTime? UpdatedAt, bool IsActive);
}
