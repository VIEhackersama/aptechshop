namespace WebApplication1.Models.DTOs;

public record RegisterDTO(
    string FullName,
    string Email,
    string Password,
    string? PhoneNumber);