using StoreOps.Models;

namespace StoreOps.Services.Customers;

public static class CustomerMappings
{
    public static CustomerDto ToDto(this Customer c) =>
        new(c.Id, c.FirstName, c.LastName, c.Email, c.Phone,
            c.AddressLine1, c.AddressLine2, c.City, c.State, c.PostalCode, c.Country,
            c.CreatedAt, c.UpdatedAt, c.IsActive);

    public static Customer ToEntity(this CreateCustomerDto d) => new()
    {
        FirstName = d.FirstName.Trim(),
        LastName = d.LastName.Trim(),
        Email = d.Email.Trim().ToLowerInvariant(),
        Phone = d.Phone?.Trim(),
        AddressLine1 = d.AddressLine1?.Trim(),
        AddressLine2 = d.AddressLine2?.Trim(),
        City = d.City?.Trim(),
        State = d.State?.Trim(),
        PostalCode = d.PostalCode?.Trim(),
        Country = d.Country?.Trim()
    };

    public static void UpdateEntity(this Customer c, UpdateCustomerDto d)
    {
        c.FirstName = d.FirstName.Trim();
        c.LastName = d.LastName.Trim();
        c.Email = d.Email.Trim().ToLowerInvariant();
        c.Phone = d.Phone?.Trim();
        c.AddressLine1 = d.AddressLine1?.Trim();
        c.AddressLine2 = d.AddressLine2?.Trim();
        c.City = d.City?.Trim();
        c.State = d.State?.Trim();
        c.PostalCode = d.PostalCode?.Trim();
        c.Country = d.Country?.Trim();
        c.IsActive = d.IsActive;
        c.UpdatedAt = DateTime.UtcNow;
    }
}
