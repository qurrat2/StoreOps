using StoreOps.Models.Enums;

namespace StoreOps.Models;

public class Coupon : BaseEntity
{
    public string Code { get; set; } = string.Empty;
    public string? Description { get; set; }
    public DiscountType DiscountType { get; set; }
    public decimal DiscountValue { get; set; }
    public decimal? MinOrderAmount { get; set; }
    public int? UsageLimit { get; set; }
    public int UsageCount { get; set; }
    public DateTime? ExpiryDate { get; set; }

    public ICollection<Order> Orders { get; set; } = new List<Order>();
}
