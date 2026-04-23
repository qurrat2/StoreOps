using StoreOps.Services.Common;

namespace StoreOps.Services.Orders;

public interface IOrderService
{
    Task<OrderDto?> GetByIdAsync(int id);
    Task<IReadOnlyList<OrderDto>> GetAllAsync(bool includeInactive = false);
    Task<Result<OrderDto>> CreateAsync(CreateOrderDto dto);
    Task<Result<OrderDto>> UpdateStatusAsync(UpdateOrderStatusDto dto);
    Task<Result<OrderDto>> MarkPaidAsync(int orderId);
    Task<Result<bool>> DeleteAsync(int id);
}
