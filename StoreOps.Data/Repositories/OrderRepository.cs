using Microsoft.EntityFrameworkCore;
using StoreOps.Models;
using StoreOps.Models.Enums;

namespace StoreOps.Data.Repositories;

public class OrderRepository : Repository<Order>, IOrderRepository
{
    public OrderRepository(IDbContextFactory<AppDbContext> factory) : base(factory)
    {
    }

    public async Task<Order?> GetByIdWithDetailsAsync(int id)
    {
        using var context = await Factory.CreateDbContextAsync();
        return await context.Orders
            .Include(o => o.Customer)
            .Include(o => o.OrderItems)
                .ThenInclude(i => i.Product)
            .AsNoTracking()
            .FirstOrDefaultAsync(o => o.Id == id);
    }

    public async Task<IReadOnlyList<Order>> GetAllWithCustomerAsync(bool includeInactive = false)
    {
        using var context = await Factory.CreateDbContextAsync();
        var query = context.Orders.Include(o => o.Customer).AsNoTracking();
        if (!includeInactive) query = query.Where(o => o.IsActive);
        return await query.OrderByDescending(o => o.OrderDate).ToListAsync();
    }

    public async Task<(Order? order, string? error)> AddOrderAsync(Order order)
    {
        using var context = await Factory.CreateDbContextAsync();
        using var tx = await context.Database.BeginTransactionAsync();

        var productIds = order.OrderItems.Select(i => i.ProductId).Distinct().ToList();
        var products = await context.Products
            .Where(p => productIds.Contains(p.Id))
            .ToListAsync();

        foreach (var item in order.OrderItems)
        {
            var product = products.FirstOrDefault(p => p.Id == item.ProductId);
            if (product is null) return (null, $"Product with ID {item.ProductId} not found.");
            if (product.StockQuantity < item.Quantity)
                return (null, $"Insufficient stock for '{product.Name}'. Available: {product.StockQuantity}, requested: {item.Quantity}.");

            product.StockQuantity -= item.Quantity;
            product.UpdatedAt = DateTime.UtcNow;
        }

        context.Orders.Add(order);
        await context.SaveChangesAsync();
        await tx.CommitAsync();

        return (order, null);
    }

    public async Task<(Order? order, string? error)> UpdateStatusAsync(int orderId, OrderStatus newStatus, bool restoreStock)
    {
        using var context = await Factory.CreateDbContextAsync();
        using var tx = await context.Database.BeginTransactionAsync();

        var order = await context.Orders
            .Include(o => o.OrderItems)
            .FirstOrDefaultAsync(o => o.Id == orderId);

        if (order is null) return (null, "Order not found.");

        if (restoreStock)
        {
            var productIds = order.OrderItems.Select(i => i.ProductId).Distinct().ToList();
            var products = await context.Products
                .Where(p => productIds.Contains(p.Id))
                .ToListAsync();

            foreach (var item in order.OrderItems)
            {
                var product = products.FirstOrDefault(p => p.Id == item.ProductId);
                if (product is null) continue;
                product.StockQuantity += item.Quantity;
                product.UpdatedAt = DateTime.UtcNow;
            }
        }

        order.Status = newStatus;
        order.UpdatedAt = DateTime.UtcNow;

        await context.SaveChangesAsync();
        await tx.CommitAsync();

        return (order, null);
    }

    public async Task<Order?> MarkPaidAsync(int orderId)
    {
        using var context = await Factory.CreateDbContextAsync();
        var order = await context.Orders.FirstOrDefaultAsync(o => o.Id == orderId);
        if (order is null) return null;

        order.PaymentStatus = PaymentStatus.Paid;
        order.UpdatedAt = DateTime.UtcNow;
        await context.SaveChangesAsync();
        return order;
    }

    public async Task<string> GenerateOrderNumberAsync()
    {
        using var context = await Factory.CreateDbContextAsync();
        var today = DateTime.UtcNow.Date;
        var tomorrow = today.AddDays(1);
        var count = await context.Orders
            .Where(o => o.OrderDate >= today && o.OrderDate < tomorrow)
            .CountAsync();
        return $"ORD-{today:yyyyMMdd}-{count + 1:D3}";
    }
}
