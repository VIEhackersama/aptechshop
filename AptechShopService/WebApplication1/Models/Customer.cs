namespace WebApplication1.Models;

public class Customer : BaseEntity
{
    public string FullName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string? PhoneNumber { get; set; }
    public string PasswordHash { get; set; } = string.Empty; // Cần ignore khi trả về API
    public string? Address { get; set; }

    public ICollection<Order> Orders { get; set; } = new List<Order>();
}