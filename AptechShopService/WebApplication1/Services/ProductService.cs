using Microsoft.EntityFrameworkCore;
using WebApplication1.Models;
using WebApplication1.Models.DTOs;

namespace WebApplication1.Services;

public class ProductService : IProductService
{
    private readonly AppDbContext _context;

    public ProductService(AppDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<ProductDTO>> GetAllProductsAsync()
    {
        var products = await _context.Products
            .Include(p => p.Category)
            .ToListAsync();
        
        return products.Select(p => MapToDto(p));
    }

    public async Task<ProductDTO> GetProductByIdAsync(int id)
    {
        var product = await _context.Products
            .Include(p => p.Category)
            .FirstOrDefaultAsync(p => p.Id == id);
            
        if (product == null) throw new Exception("Product not found");
        
        return MapToDto(product);
    }

    public async Task<ProductDTO> CreateProductAsync(CreateProductDTO request)
    {
        var product = new Product
        {
            Name = request.Name,
            Sku = request.Sku,
            Description = request.Description,
            Price = request.Price,
            StockQuantity = request.StockQuantity,
            ImageUrl = request.ImageUrl,
            CategoryId = request.CategoryId,
            IsActive = true
        };

        _context.Products.Add(product);
        await _context.SaveChangesAsync();
        
        // Reload to get Category info if needed, or just return basic
        await _context.Entry(product).Reference(p => p.Category).LoadAsync();

        return MapToDto(product);
    }

    public async Task<ProductDTO> UpdateProductAsync(int id, CreateProductDTO request)
    {
        var product = await _context.Products.FindAsync(id);
        if (product == null) throw new Exception("Product not found");

        product.Name = request.Name;
        product.Sku = request.Sku;
        product.Description = request.Description;
        product.Price = request.Price;
        product.StockQuantity = request.StockQuantity;
        product.ImageUrl = request.ImageUrl;
        product.CategoryId = request.CategoryId;

        await _context.SaveChangesAsync();
        await _context.Entry(product).Reference(p => p.Category).LoadAsync();

        return MapToDto(product);
    }

    public async Task DeleteProductAsync(int id)
    {
        var product = await _context.Products.FindAsync(id);
        if (product == null) throw new Exception("Product not found");
        
        _context.Products.Remove(product);
        await _context.SaveChangesAsync();
    }

    private static ProductDTO MapToDto(Product product)
    {
        return new ProductDTO(
            Id: product.Id,
            Name: product.Name,
            Sku: product.Sku,
            Price: product.Price,
            StockQuantity: product.StockQuantity,
            ImageUrl: product.ImageUrl,
            CategoryName: product.Category?.Name ?? "Unknown",
            CategoryId: product.CategoryId
        );
    }
}
