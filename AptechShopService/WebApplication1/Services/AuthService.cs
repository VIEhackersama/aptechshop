using BCrypt.Net;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using WebApplication1.Models;
using WebApplication1.Models.DTOs;

namespace WebApplication1.Services;

public class AuthService
{
    private readonly AppDbContext _context;
    private readonly IConfiguration _configuration;

    public AuthService(AppDbContext context, IConfiguration configuration)
    {
        _context = context;
        _configuration = configuration;
    }

    public async Task<AuthResponseDto> RegisterAsync(RegisterDTO request)
    {
        var existingUser = await _context.Customers.FirstOrDefaultAsync(c => c.Email == request.Email);
        if (existingUser != null)
        {
            throw new Exception("Email already exists.");
        }

        var passwordHash = BCrypt.Net.BCrypt.HashPassword(request.Password);

        var newCustomer = new Customer
        {
            FullName = request.FullName,
            Email = request.Email,
            PasswordHash = passwordHash,
            PhoneNumber = "N/A", 
            Address = "N/A"      
        };

        _context.Customers.Add(newCustomer);
        await _context.SaveChangesAsync();

        var token = GenerateJwtToken(newCustomer);

        return new AuthResponseDto(newCustomer.Id.ToString(), newCustomer.Email, token);
    }

    public async Task<AuthResponseDto> LoginAsync(LoginDTO request)
    {
        var customer = await _context.Customers.FirstOrDefaultAsync(c => c.Email == request.Email);
        if (customer == null)
        {
            throw new Exception("Invalid credentials.");
        }

        if (!BCrypt.Net.BCrypt.Verify(request.Password, customer.PasswordHash))
        {
            throw new Exception("Invalid credentials.");
        }

        var token = GenerateJwtToken(customer);

        return new AuthResponseDto(customer.Id.ToString(), customer.Email, token);
    }

    private string GenerateJwtToken(Customer customer)
    {
        var key = Encoding.ASCII.GetBytes(_configuration["Jwt:Key"]!);
        var tokenHandler = new JwtSecurityTokenHandler();
        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(new[]
            {
                new Claim("id", customer.Id.ToString()),
                new Claim(ClaimTypes.Name, customer.FullName),
                new Claim(ClaimTypes.Email, customer.Email)
            }),
            Expires = DateTime.UtcNow.AddDays(7),
            Issuer = _configuration["Jwt:Issuer"],
            Audience = _configuration["Jwt:Audience"],
            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
        };

        var token = tokenHandler.CreateToken(tokenDescriptor);
        return tokenHandler.WriteToken(token);
    }
}
