using StoreOps.Models;

namespace StoreOps.Services.Orders;

public static class OrderMappings
{
    public static OrderDto ToDto(this Order o) => new(
        o.Id,
        o.OrderNumber,
        o.OrderDate,
        o.CustomerId,
        o.Customer is null ? string.Empty : $"{o.Customer.FirstName} {o.Customer.LastName}".Trim(),
        o.Status,
        o.PaymentStatus,
        o.Subtotal,
        o.ShippingAmount,
        o.DiscountAmount,
        o.TotalAmount,
        o.Notes,
        (o.OrderItems ?? new List<OrderItem>()).Select(i => i.ToDto()).ToList(),
        o.CreatedAt,
        o.UpdatedAt,
        o.IsActive);

    public static OrderItemDto ToDto(this OrderItem i) => new(
        i.Id,
        i.ProductId,
        i.Product?.Sku ?? string.Empty,
        i.Product?.Name ?? string.Empty,
        i.Quantity,
        i.UnitPrice,
        i.LineTotal);
}
