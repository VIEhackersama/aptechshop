namespace WebApplication1.Models.DTOs;

public record CreateOrderDTO(
    string ShippingAddress,
    string? PaymentMethod,
    string? Note,
    List<CartItemDTO> Items // Danh sách sản phẩm mua
    );