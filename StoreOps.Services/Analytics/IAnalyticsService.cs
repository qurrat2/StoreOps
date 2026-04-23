namespace StoreOps.Services.Analytics;

public interface IAnalyticsService
{
    Task<IReadOnlyList<TopProductDto>> GetTopSellingProductsAsync(int days, int count);
    Task<IReadOnlyList<TrendPointDto>> GetWeeklyTrendAsync(int weeks);
}
