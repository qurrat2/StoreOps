using StoreOps.Models.Enums;

namespace StoreOps.Services.Orders;

public record UpdateOrderStatusDto
{
    public int Id { get; set; }
    public OrderStatus NewStatus { get; set; }
}
