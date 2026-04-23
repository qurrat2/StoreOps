using StoreOps.Data.Repositories;
using StoreOps.Services.Common;

namespace StoreOps.Services.Categories;

public class CategoryService : ICategoryService
{
    private readonly ICategoryRepository _repository;

    public CategoryService(ICategoryRepository repository)
    {
        _repository = repository;
    }

    public async Task<CategoryDto?> GetByIdAsync(int id)
    {
        var entity = await _repository.GetByIdAsync(id);
        return entity?.ToDto();
    }

    public async Task<IReadOnlyList<CategoryDto>> GetAllAsync(bool includeInactive = false)
    {
        var entities = await _repository.GetAllAsync(includeInactive);
        return entities.Select(e => e.ToDto()).ToList();
    }

    public async Task<Result<CategoryDto>> CreateAsync(CreateCategoryDto dto)
    {
        if (string.IsNullOrWhiteSpace(dto.Name))
        {
            return Result<CategoryDto>.Failure("Name is required.");
        }

        if (dto.Name.Length > 100)
        {
            return Result<CategoryDto>.Failure("Name must be 100 characters or fewer.");
        }

        var normalizedName = dto.Name.Trim();

        if (await _repository.NameExistsAsync(normalizedName))
        {
            return Result<CategoryDto>.Failure($"A category named '{normalizedName}' already exists.");
        }

        var entity = dto.ToEntity();
        var created = await _repository.AddAsync(entity);
        return Result<CategoryDto>.Success(created.ToDto());
    }

    public async Task<Result<CategoryDto>> UpdateAsync(UpdateCategoryDto dto)
    {
        if (string.IsNullOrWhiteSpace(dto.Name))
        {
            return Result<CategoryDto>.Failure("Name is required.");
        }

        if (dto.Name.Length > 100)
        {
            return Result<CategoryDto>.Failure("Name must be 100 characters or fewer.");
        }

        var entity = await _repository.GetByIdAsync(dto.Id);
        if (entity is null)
        {
            return Result<CategoryDto>.Failure("Category not found.");
        }

        var normalizedName = dto.Name.Trim();

        if (await _repository.NameExistsAsync(normalizedName, excludeId: dto.Id))
        {
            return Result<CategoryDto>.Failure($"A category named '{normalizedName}' already exists.");
        }

        dto.UpdateEntity(entity);
        var updated = await _repository.UpdateAsync(entity);
        return Result<CategoryDto>.Success(updated.ToDto());
    }

    public async Task<Result<bool>> DeleteAsync(int id)
    {
        if (!await _repository.ExistsAsync(id))
        {
            return Result<bool>.Failure("Category not found.");
        }

        await _repository.DeleteAsync(id);
        return Result<bool>.Success(true);
    }
}
