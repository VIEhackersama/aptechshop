using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using WebApplication1.Models;
using WebApplication1.Models.DTOs;
using WebApplication1.Services;

public class OrderService : IOrderService
{
    private readonly AppDbContext _context;

    public OrderService(AppDbContext context)
    {
        _context = context;
    }

    public async Task<OrderDetailDTO> CreateOrderAsync(long customerId, CreateOrderDTO request)
    {
        // 1. Bắt đầu Transaction (Quan trọng)
        // Dùng 'using' để đảm bảo dispose sau khi xong
        using var transaction = await _context.Database.BeginTransactionAsync();

        try
        {
            // 2. Khởi tạo Order Header
            var order = new Order
            {
                CustomerId = customerId,
                OrderDate = DateTimeOffset.UtcNow,
                Status = OrderStatus.Pending,
                ShippingAddress = request.ShippingAddress,
                PaymentMethod = request.PaymentMethod,
                Note = request.Note,
                TotalAmount = 0 // Sẽ cộng dồn bên dưới
            };

            // 3. Xử lý từng sản phẩm trong giỏ hàng
            foreach (var item in request.Items)
            {
                // Query sản phẩm từ DB (để lấy giá chuẩn và check tồn kho)
                // Sử dụng Lock hoặc check kỹ ở đây nếu cần high concurrency (nhưng scale vừa thì check thường ok)
                var product = await _context.Products.FindAsync(item.ProductId);

                // Validation
                if (product == null)
                {
                    throw new Exception($"Sản phẩm với ID {item.ProductId} không tồn tại.");
                }
                
                if (!product.IsActive)
                {
                    throw new Exception($"Sản phẩm {product.Name} hiện đang ngừng kinh doanh.");
                }

                if (product.StockQuantity < item.Quantity)
                {
                    throw new Exception($"Sản phẩm {product.Name} không đủ số lượng tồn kho (Còn: {product.StockQuantity}).");
                }

                // 4. Trừ tồn kho
                product.StockQuantity -= item.Quantity;

                // 5. Tạo OrderItem
                var orderItem = new OrderItem
                {
                    Product = product, // Gán reference để EF tự hiểu
                    Quantity = item.Quantity,
                    UnitPrice = product.Price // LƯU Ý: Lấy giá từ DB, không lấy từ DTO
                    // TotalPrice sẽ được DB tự tính (Generated Column)
                };

                order.OrderItems.Add(orderItem);
                
                // Cộng dồn tổng tiền đơn hàng
                order.TotalAmount += (product.Price * item.Quantity);
            }

            // 6. Lưu vào DB
            _context.Orders.Add(order);
            await _context.SaveChangesAsync();

            // 7. Commit Transaction (Xác nhận mọi thứ thành công)
            await transaction.CommitAsync();

            // 8. Return kết quả (Mapping sang DTO)
            return MapToDto(order);
        }
        catch (Exception)
        {
            // Nếu có lỗi, Rollback mọi thay đổi (trả lại tồn kho, hủy đơn)
            await transaction.RollbackAsync();
            throw; // Ném lỗi ra để Controller bắt và trả về HTTP 400/500
        }
    }

    public async Task<OrderDetailDTO> GetOrderByIdAsync(long orderId, long customerId)
    {
        var order = await _context.Orders
            .Include(o => o.OrderItems)
            .ThenInclude(oi => oi.Product) // Load kèm thông tin Product để lấy tên
            .FirstOrDefaultAsync(o => o.Id == orderId && o.CustomerId == customerId);

        if (order == null)
        {
            throw new Exception("Không tìm thấy đơn hàng.");
        }

        return MapToDto(order);
    }

    // Helper method để map Entity -> DTO (Có thể thay bằng AutoMapper)
    private static OrderDetailDTO MapToDto(Order order)
    {
        return new OrderDetailDTO(
            Id: order.Id,
            OrderDate: order.OrderDate,
            Status: order.Status.ToString(),
            TotalAmount: order.TotalAmount,
            ShippingAddress: order.ShippingAddress,
            Items: order.OrderItems.Select(oi => new OrderItemDTO(
                ProductName: oi.Product.Name,
                Quantity: oi.Quantity,
                UnitPrice: oi.UnitPrice,
                TotalPrice: oi.Quantity * oi.UnitPrice // Tính tạm ở code để trả về
            )).ToList()
        );
    }
}