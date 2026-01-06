using Microsoft.AspNetCore.Mvc;
using WebApplication1.Models.DTOs;
using WebApplication1.Services;

namespace WebApplication1.Controllers;
[ApiController]
[Route("api/[controller]")]
//[Authorize] // Yêu cầu phải đăng nhập (JWT)
public class OrdersController : ControllerBase
{
    private readonly IOrderService _orderService;

    public OrdersController(IOrderService orderService)
    {
        _orderService = orderService;
    }

    [HttpPost]
    public async Task<IActionResult> CreateOrder([FromBody] CreateOrderDTO request)
    {
        try
        {
            // Lấy CustomerId từ Token (Claim)
            var userIdClaim = User.FindFirst("id"); // Hoặc ClaimTypes.NameIdentifier tùy cách bạn config JWT
            if (userIdClaim == null) return Unauthorized();
            
            long customerId = long.Parse(userIdClaim.Value);

            var result = await _orderService.CreateOrderAsync(customerId, request);
            return Ok(result);
        }
        catch (Exception ex)
        {
            // Trong thực tế nên dùng Middleware để handle error global
            return BadRequest(new { message = ex.Message });
        }
    }
    
    [HttpGet("{id}")]
    public async Task<IActionResult> GetOrder(long id)
    {
        var userIdClaim = User.FindFirst("id");
        long customerId = long.Parse(userIdClaim.Value);
        
        try 
        {
            var order = await _orderService.GetOrderByIdAsync(id, customerId);
            return Ok(order);
        }
        catch(Exception ex)
        {
            return NotFound(ex.Message);
        }
    }
}