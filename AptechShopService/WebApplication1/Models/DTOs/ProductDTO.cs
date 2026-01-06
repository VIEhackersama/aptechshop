namespace WebApplication1.Models.DTOs;

public record ProductDTO(
    int Id,
    string Name,
    string? Sku,
    decimal Price,
    int StockQuantity,
    string? ImageUrl,
    string CategoryName,
    int CategoryId
);
