namespace StoreOps.Services.Analytics;

public record TrendPointDto(
    DateTime BucketStart,
    string Label,
    int OrderCount,
    decimal Revenue);
