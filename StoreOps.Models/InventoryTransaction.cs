using StoreOps.Models.Enums;

namespace StoreOps.Models;

public class InventoryTransaction : BaseEntity
{
    public int ProductId { get; set; }
    public Product Product { get; set; } = null!;

    public InventoryTransactionType TransactionType { get; set; }
    public int QuantityChange { get; set; }
    public int QuantityAfter { get; set; }
    public string? Notes { get; set; }
    public string? Reference { get; set; }
}
