namespace WebApplication1.Models;

public class Order : BaseEntity
{
    public long OrderId { get; set; }   // or Id, or something else
    public DateTimeOffset OrderDate { get; set; } = DateTimeOffset.UtcNow;
    public OrderStatus Status { get; set; } = OrderStatus.Pending;
    public decimal TotalAmount { get; set; }
    public string ShippingAddress { get; set; } = string.Empty;
    public string? PaymentMethod { get; set; }
    public string? Note { get; set; }

    public long? CustomerId { get; set; }
    public Customer? Customer { get; set; }
    
    public ICollection<OrderItem> OrderItems { get; set; } = new List<OrderItem>();
}