namespace WebApplication1.Models.DTOs;

public record CreateProductDTO(string Name,
    string Sku,
    decimal Price,
    int StockQuantity,
    int CategoryId,
    string? Description,
    string? ImageUrl);