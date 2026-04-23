using Microsoft.EntityFrameworkCore;
using StoreOps.Models;

namespace StoreOps.Data.Repositories;

public class CustomerRepository : Repository<Customer>, ICustomerRepository
{
    public CustomerRepository(IDbContextFactory<AppDbContext> factory) : base(factory)
    {
    }

    public async Task<Customer?> GetByEmailAsync(string email)
    {
        using var context = await Factory.CreateDbContextAsync();
        return await context.Customers
            .AsNoTracking()
            .FirstOrDefaultAsync(c => c.Email == email);
    }

    public async Task<bool> EmailExistsAsync(string email, int? excludeId = null)
    {
        using var context = await Factory.CreateDbContextAsync();
        var query = context.Customers.Where(c => c.Email == email);

        if (excludeId.HasValue)
        {
            query = query.Where(c => c.Id != excludeId.Value);
        }

        return await query.AnyAsync();
    }
}
