namespace WebApplication1.Models.DTOs;

public record OrderItemDTO(
    string ProductName,
    int Quantity,
    decimal UnitPrice,
    decimal TotalPrice
    );