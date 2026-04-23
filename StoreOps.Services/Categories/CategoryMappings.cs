using StoreOps.Models;

namespace StoreOps.Services.Categories;

public static class CategoryMappings
{
    public static CategoryDto ToDto(this Category entity)
    {
        return new CategoryDto(
            entity.Id,
            entity.Name,
            entity.Description,
            entity.IsActive,
            entity.CreatedAt,
            entity.UpdatedAt
        );
    }

    public static Category ToEntity(this CreateCategoryDto dto)
    {
        return new Category
        {
            Name = dto.Name.Trim(),
            Description = dto.Description?.Trim()
        };
    }

    public static void UpdateEntity(this UpdateCategoryDto dto, Category entity)
    {
        entity.Name = dto.Name.Trim();
        entity.Description = dto.Description?.Trim();
        entity.IsActive = dto.IsActive;
    }
}
