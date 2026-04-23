using StoreOps.Models.Enums;

namespace StoreOps.Services.Orders;

public record OrderDto(
    int Id,
    string OrderNumber,
    DateTime OrderDate,
    int CustomerId,
    string CustomerName,
    OrderStatus Status,
    PaymentStatus PaymentStatus,
    decimal Subtotal,
    decimal ShippingAmount,
    decimal DiscountAmount,
    decimal TotalAmount,
    string? Notes,
    IReadOnlyList<OrderItemDto> Items,
    DateTime CreatedAt,
    DateTime? UpdatedAt,
    bool IsActive);

public record OrderItemDto(
    int Id,
    int ProductId,
    string ProductSku,
    string ProductName,
    int Quantity,
    decimal UnitPrice,
    decimal LineTotal);
