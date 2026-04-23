using System.ComponentModel.DataAnnotations;

namespace StoreOps.Services.Orders;

public record CreateOrderDto
{
    [Range(1, int.MaxValue, ErrorMessage = "Select a customer.")]
    public int CustomerId { get; set; }

    [Range(0, double.MaxValue, ErrorMessage = "Shipping cannot be negative.")]
    public decimal ShippingAmount { get; set; }

    [Range(0, double.MaxValue, ErrorMessage = "Discount cannot be negative.")]
    public decimal DiscountAmount { get; set; }

    [MaxLength(2000)]
    public string? Notes { get; set; }

    public List<CreateOrderItemDto> Items { get; set; } = new();
}

public record CreateOrderItemDto
{
    [Range(1, int.MaxValue, ErrorMessage = "Select a product.")]
    public int ProductId { get; set; }

    [Range(1, int.MaxValue, ErrorMessage = "Quantity must be at least 1.")]
    public int Quantity { get; set; } = 1;
}
