namespace WebApplication1.Models;

public class Product : BaseEntity
{
    public string Name { get; set; } = string.Empty;
    public string? Sku { get; set; }
    public string? Description { get; set; }
    public decimal Price { get; set; }
    public int StockQuantity { get; set; }
    public string? ImageUrl { get; set; }
    public bool IsActive { get; set; } = true;

    // Foreign Key
    public int CategoryId { get; set; }
    public Category Category { get; set; } = null!;
}