namespace StoreOps.Services.Analytics;

public record TopProductDto(
    int ProductId,
    string ProductName,
    string ProductSku,
    int TotalQuantity,
    decimal TotalRevenue);
