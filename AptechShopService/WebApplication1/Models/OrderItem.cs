namespace WebApplication1.Models;

public class OrderItem : BaseEntity
{
    // public long Id { get; set; }
    
    public long OrderId { get; set; }
    public Order Order { get; set; } = null!;

    public long ProductId { get; set; }
    public Product Product { get; set; } = null!;

    public int Quantity { get; set; }
    public decimal UnitPrice { get; set; } // Giá snapshot
    public decimal TotalPrice { get; set; } // Database tự tính, nhưng C# cần để hứng dữ liệu đọc lên
}