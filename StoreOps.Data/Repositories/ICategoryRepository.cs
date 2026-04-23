using StoreOps.Models;

namespace StoreOps.Data.Repositories;

public interface ICategoryRepository : IRepository<Category>
{
    Task<Category?> GetByNameAsync(string name);
    Task<bool> NameExistsAsync(string name, int? excludeId = null);
}
