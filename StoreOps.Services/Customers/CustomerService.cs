using StoreOps.Data.Repositories;
using StoreOps.Services.Common;

namespace StoreOps.Services.Customers;

public class CustomerService : ICustomerService
{
    private readonly ICustomerRepository _repository;

    public CustomerService(ICustomerRepository repository)
    {
        _repository = repository;
    }

    public async Task<CustomerDto?> GetByIdAsync(int id)
    {
        var entity = await _repository.GetByIdAsync(id);
        return entity?.ToDto();
    }

    public async Task<IReadOnlyList<CustomerDto>> GetAllAsync(bool includeInactive = false)
    {
        var entities = await _repository.GetAllAsync(includeInactive);
        return entities.Select(e => e.ToDto()).ToList();
    }

    public async Task<Result<CustomerDto>> CreateAsync(CreateCustomerDto dto)
    {
        var validation = Validate(dto.FirstName, dto.LastName, dto.Email);
        if (validation is not null) return Result<CustomerDto>.Failure(validation);

        var normalizedEmail = dto.Email.Trim().ToLowerInvariant();
        if (await _repository.EmailExistsAsync(normalizedEmail))
        {
            return Result<CustomerDto>.Failure($"A customer with email '{normalizedEmail}' already exists.");
        }

        var entity = dto.ToEntity();
        var created = await _repository.AddAsync(entity);
        return Result<CustomerDto>.Success(created.ToDto());
    }

    public async Task<Result<CustomerDto>> UpdateAsync(UpdateCustomerDto dto)
    {
        var validation = Validate(dto.FirstName, dto.LastName, dto.Email);
        if (validation is not null) return Result<CustomerDto>.Failure(validation);

        var entity = await _repository.GetByIdAsync(dto.Id);
        if (entity is null)
        {
            return Result<CustomerDto>.Failure("Customer not found.");
        }

        var normalizedEmail = dto.Email.Trim().ToLowerInvariant();
        if (await _repository.EmailExistsAsync(normalizedEmail, excludeId: dto.Id))
        {
            return Result<CustomerDto>.Failure($"A customer with email '{normalizedEmail}' already exists.");
        }

        entity.UpdateEntity(dto);
        var updated = await _repository.UpdateAsync(entity);
        return Result<CustomerDto>.Success(updated.ToDto());
    }

    public async Task<Result<bool>> DeleteAsync(int id)
    {
        if (!await _repository.ExistsAsync(id))
        {
            return Result<bool>.Failure("Customer not found.");
        }

        await _repository.DeleteAsync(id);
        return Result<bool>.Success(true);
    }

    private static string? Validate(string firstName, string lastName, string email)
    {
        if (string.IsNullOrWhiteSpace(firstName)) return "First name is required.";
        if (firstName.Length > 100) return "First name must be 100 characters or fewer.";
        if (string.IsNullOrWhiteSpace(lastName)) return "Last name is required.";
        if (lastName.Length > 100) return "Last name must be 100 characters or fewer.";
        if (string.IsNullOrWhiteSpace(email)) return "Email is required.";
        if (email.Length > 256) return "Email must be 256 characters or fewer.";
        if (!email.Contains('@')) return "Email is not in a valid format.";
        return null;
    }
}
