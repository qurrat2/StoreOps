using StoreOps.Data.Repositories;
using StoreOps.Models;
using StoreOps.Models.Enums;
using StoreOps.Services.Common;

namespace StoreOps.Services.Orders;

public class OrderService : IOrderService
{
    private readonly IOrderRepository _orders;
    private readonly IProductRepository _products;
    private readonly ICustomerRepository _customers;

    public OrderService(IOrderRepository orders, IProductRepository products, ICustomerRepository customers)
    {
        _orders = orders;
        _products = products;
        _customers = customers;
    }

    public async Task<OrderDto?> GetByIdAsync(int id)
    {
        var entity = await _orders.GetByIdWithDetailsAsync(id);
        return entity?.ToDto();
    }

    public async Task<IReadOnlyList<OrderDto>> GetAllAsync(bool includeInactive = false)
    {
        var entities = await _orders.GetAllWithCustomerAsync(includeInactive);
        return entities.Select(e => e.ToDto()).ToList();
    }

    public async Task<Result<OrderDto>> CreateAsync(CreateOrderDto dto)
    {
        if (dto.Items.Count == 0)
            return Result<OrderDto>.Failure("At least one line item is required.");

        if (dto.Items.Any(i => i.Quantity < 1))
            return Result<OrderDto>.Failure("Each line item must have quantity of at least 1.");

        if (!await _customers.ExistsAsync(dto.CustomerId))
            return Result<OrderDto>.Failure("Selected customer does not exist.");

        var productIds = dto.Items.Select(i => i.ProductId).Distinct().ToList();
        var products = await _products.GetByIdsAsync(productIds);

        foreach (var item in dto.Items)
        {
            var product = products.FirstOrDefault(p => p.Id == item.ProductId);
            if (product is null)
                return Result<OrderDto>.Failure($"Product with ID {item.ProductId} not found.");
            if (!product.IsActive)
                return Result<OrderDto>.Failure($"Product '{product.Name}' is inactive.");
        }

        var orderItems = dto.Items.Select(item =>
        {
            var product = products.First(p => p.Id == item.ProductId);
            return new OrderItem
            {
                ProductId = item.ProductId,
                Quantity = item.Quantity,
                UnitPrice = product.Price,
                LineTotal = product.Price * item.Quantity
            };
        }).ToList();

        var subtotal = orderItems.Sum(i => i.LineTotal);
        var total = subtotal + dto.ShippingAmount - dto.DiscountAmount;
        if (total < 0)
            return Result<OrderDto>.Failure("Order total cannot be negative.");

        var orderNumber = await _orders.GenerateOrderNumberAsync();

        var order = new Order
        {
            OrderNumber = orderNumber,
            OrderDate = DateTime.UtcNow,
            CustomerId = dto.CustomerId,
            Status = OrderStatus.Pending,
            PaymentStatus = PaymentStatus.Pending,
            Subtotal = subtotal,
            ShippingAmount = dto.ShippingAmount,
            DiscountAmount = dto.DiscountAmount,
            TotalAmount = total,
            Notes = dto.Notes?.Trim(),
            OrderItems = orderItems
        };

        var (created, error) = await _orders.AddOrderAsync(order);
        if (error is not null) return Result<OrderDto>.Failure(error);

        var full = await _orders.GetByIdWithDetailsAsync(created!.Id);
        return Result<OrderDto>.Success(full!.ToDto());
    }

    public async Task<Result<OrderDto>> UpdateStatusAsync(UpdateOrderStatusDto dto)
    {
        var order = await _orders.GetByIdWithDetailsAsync(dto.Id);
        if (order is null)
            return Result<OrderDto>.Failure("Order not found.");

        if (!IsValidTransition(order.Status, dto.NewStatus))
            return Result<OrderDto>.Failure($"Cannot change status from {order.Status} to {dto.NewStatus}.");

        var restoreStock = dto.NewStatus == OrderStatus.Cancelled && order.Status != OrderStatus.Cancelled;

        var (updated, error) = await _orders.UpdateStatusAsync(dto.Id, dto.NewStatus, restoreStock);
        if (error is not null) return Result<OrderDto>.Failure(error);

        var full = await _orders.GetByIdWithDetailsAsync(updated!.Id);
        return Result<OrderDto>.Success(full!.ToDto());
    }

    public async Task<Result<OrderDto>> MarkPaidAsync(int orderId)
    {
        var order = await _orders.GetByIdWithDetailsAsync(orderId);
        if (order is null)
            return Result<OrderDto>.Failure("Order not found.");

        if (order.PaymentStatus == PaymentStatus.Paid)
            return Result<OrderDto>.Failure("Order is already marked as paid.");

        if (order.Status == OrderStatus.Cancelled)
            return Result<OrderDto>.Failure("Cannot mark a cancelled order as paid.");

        var updated = await _orders.MarkPaidAsync(orderId);
        if (updated is null) return Result<OrderDto>.Failure("Order not found.");

        var full = await _orders.GetByIdWithDetailsAsync(updated.Id);
        return Result<OrderDto>.Success(full!.ToDto());
    }

    public async Task<Result<bool>> DeleteAsync(int id)
    {
        if (!await _orders.ExistsAsync(id))
            return Result<bool>.Failure("Order not found.");

        await _orders.DeleteAsync(id);
        return Result<bool>.Success(true);
    }

    private static bool IsValidTransition(OrderStatus from, OrderStatus to)
    {
        if (from == to) return false;
        if (from == OrderStatus.Delivered || from == OrderStatus.Cancelled) return false;
        if (to == OrderStatus.Cancelled) return true;
        return (from, to) switch
        {
            (OrderStatus.Pending, OrderStatus.Processing) => true,
            (OrderStatus.Processing, OrderStatus.Shipped) => true,
            (OrderStatus.Shipped, OrderStatus.Delivered) => true,
            _ => false
        };
    }
}
