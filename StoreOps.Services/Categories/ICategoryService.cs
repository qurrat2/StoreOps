using StoreOps.Services.Common;

namespace StoreOps.Services.Categories;

public interface ICategoryService
{
    Task<CategoryDto?> GetByIdAsync(int id);
    Task<IReadOnlyList<CategoryDto>> GetAllAsync(bool includeInactive = false);
    Task<Result<CategoryDto>> CreateAsync(CreateCategoryDto dto);
    Task<Result<CategoryDto>> UpdateAsync(UpdateCategoryDto dto);
    Task<Result<bool>> DeleteAsync(int id);
}
