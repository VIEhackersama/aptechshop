using System.Collections.Generic;
using System.Threading.Tasks;
using WebApplication1.Models.DTOs;

namespace WebApplication1.Services;

public interface IProductService
{
    Task<IEnumerable<ProductDTO>> GetAllProductsAsync();
    Task<ProductDTO> GetProductByIdAsync(int id);
    Task<ProductDTO> CreateProductAsync(CreateProductDTO request);
    Task<ProductDTO> UpdateProductAsync(int id, CreateProductDTO request);
    Task DeleteProductAsync(int id);
}
