namespace StoreOps.Services.Customers;

public record CustomerDto(
    int Id,
    string FirstName,
    string LastName,
    string Email,
    string? Phone,
    string? AddressLine1,
    string? AddressLine2,
    string? City,
    string? State,
    string? PostalCode,
    string? Country,
    DateTime CreatedAt,
    DateTime? UpdatedAt,
    bool IsActive)
{
    public string FullName => $"{FirstName} {LastName}".Trim();
}
