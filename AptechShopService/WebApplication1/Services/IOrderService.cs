using WebApplication1.Models.DTOs;

namespace WebApplication1.Services;

public interface IOrderService
{
    Task<OrderDetailDTO> CreateOrderAsync(long customerId, CreateOrderDTO request);
    
    Task<OrderDetailDTO> GetOrderByIdAsync(long orderId, long customerId);
}