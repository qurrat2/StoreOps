using StoreOps.Services.Common;

namespace StoreOps.Services.Customers;

public interface ICustomerService
{
    Task<CustomerDto?> GetByIdAsync(int id);
    Task<IReadOnlyList<CustomerDto>> GetAllAsync(bool includeInactive = false);
    Task<Result<CustomerDto>> CreateAsync(CreateCustomerDto dto);
    Task<Result<CustomerDto>> UpdateAsync(UpdateCustomerDto dto);
    Task<Result<bool>> DeleteAsync(int id);
}
