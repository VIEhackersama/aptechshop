namespace WebApplication1.Models.DTOs;

public record OrderDetailDTO(
    long Id,
    DateTimeOffset OrderDate,
    string Status,
    decimal TotalAmount,
    string ShippingAddress,
    List<OrderItemDTO> Items);