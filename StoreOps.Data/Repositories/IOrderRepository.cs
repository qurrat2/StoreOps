using StoreOps.Models;
using StoreOps.Models.Enums;

namespace StoreOps.Data.Repositories;

public interface IOrderRepository : IRepository<Order>
{
    Task<Order?> GetByIdWithDetailsAsync(int id);
    Task<IReadOnlyList<Order>> GetAllWithCustomerAsync(bool includeInactive = false);
    Task<(Order? order, string? error)> AddOrderAsync(Order order);
    Task<(Order? order, string? error)> UpdateStatusAsync(int orderId, OrderStatus newStatus, bool restoreStock);
    Task<Order?> MarkPaidAsync(int orderId);
    Task<string> GenerateOrderNumberAsync();
}
