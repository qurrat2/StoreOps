using StoreOps.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace StoreOps.Services.Products
{
    public static class ProductMappings
    {
        public static ProductDto ToDto(this Product p) =>
      new(p.Id, p.Sku, p.Name, p.Description, p.Price, p.CostPrice,
          p.StockQuantity, p.ReorderLevel, p.ImageUrl,
          p.CategoryId, p.Category?.Name ?? string.Empty,
          p.CreatedAt, p.UpdatedAt, p.IsActive);

        public static Product ToEntity(this CreateProductDto d) => new()
        {
            Sku = d.Sku.Trim().ToUpperInvariant(),
            Name = d.Name.Trim(),
            Description = d.Description?.Trim(),
            Price = d.Price,
            CostPrice = d.CostPrice,
            StockQuantity = d.StockQuantity,
            ReorderLevel = d.ReorderLevel,
            ImageUrl = d.ImageUrl?.Trim(),
            CategoryId = d.CategoryId
        };

        public static void UpdateEntity(this Product p, UpdateProductDto d)
        {
            p.Sku = d.Sku.Trim().ToUpperInvariant();
            p.Name = d.Name.Trim();
            p.Description = d.Description?.Trim();
            p.Price = d.Price;
            p.CostPrice = d.CostPrice;
            p.StockQuantity = d.StockQuantity;
            p.ReorderLevel = d.ReorderLevel;
            p.ImageUrl = d.ImageUrl?.Trim();
            p.CategoryId = d.CategoryId;
            p.IsActive = d.IsActive;
            p.UpdatedAt = DateTime.UtcNow;
        }

    }
}
