using System.Globalization;
using Microsoft.EntityFrameworkCore;
using StoreOps.Data;
using StoreOps.Models.Enums;

namespace StoreOps.Services.Analytics;

public class AnalyticsService : IAnalyticsService
{
    private readonly IDbContextFactory<AppDbContext> _factory;

    public AnalyticsService(IDbContextFactory<AppDbContext> factory)
    {
        _factory = factory;
    }

    public async Task<IReadOnlyList<TopProductDto>> GetTopSellingProductsAsync(int days, int count)
    {
        using var context = await _factory.CreateDbContextAsync();
        var since = DateTime.UtcNow.Date.AddDays(-days);

        var rows = await context.OrderItems
            .Where(i => i.Order.OrderDate >= since && i.Order.Status != OrderStatus.Cancelled)
            .GroupBy(i => new { i.ProductId, i.Product.Name, i.Product.Sku })
            .Select(g => new
            {
                g.Key.ProductId,
                g.Key.Name,
                g.Key.Sku,
                TotalQuantity = g.Sum(i => i.Quantity),
                TotalRevenue = g.Sum(i => i.LineTotal)
            })
            .OrderByDescending(r => r.TotalQuantity)
            .Take(count)
            .ToListAsync();

        return rows
            .Select(r => new TopProductDto(r.ProductId, r.Name, r.Sku, r.TotalQuantity, r.TotalRevenue))
            .ToList();
    }

    public async Task<IReadOnlyList<TrendPointDto>> GetWeeklyTrendAsync(int weeks)
    {
        using var context = await _factory.CreateDbContextAsync();
        var today = DateTime.UtcNow.Date;
        var startOfWeek = today.AddDays(-(int)today.DayOfWeek); // Sunday start
        var firstBucket = startOfWeek.AddDays(-7 * (weeks - 1));

        var orders = await context.Orders
            .Where(o => o.OrderDate >= firstBucket && o.Status != OrderStatus.Cancelled)
            .Select(o => new { o.OrderDate, o.TotalAmount, o.PaymentStatus })
            .ToListAsync();

        var buckets = new List<TrendPointDto>(weeks);
        for (int w = 0; w < weeks; w++)
        {
            var bucketStart = firstBucket.AddDays(7 * w);
            var bucketEnd = bucketStart.AddDays(7);
            var inBucket = orders.Where(o => o.OrderDate >= bucketStart && o.OrderDate < bucketEnd).ToList();

            var label = bucketStart.ToString("MMM d", CultureInfo.InvariantCulture);
            var count = inBucket.Count;
            var revenue = inBucket.Where(o => o.PaymentStatus == PaymentStatus.Paid).Sum(o => o.TotalAmount);

            buckets.Add(new TrendPointDto(bucketStart, label, count, revenue));
        }

        return buckets;
    }
}
