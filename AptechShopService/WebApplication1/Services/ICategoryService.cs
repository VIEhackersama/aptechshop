using System.Collections.Generic;
using System.Threading.Tasks;
using WebApplication1.Models;
using WebApplication1.Models.DTOs;

namespace WebApplication1.Services;

public interface ICategoryService
{
    Task<IEnumerable<Category>> GetAllCategoriesAsync();
    Task<Category> GetCategoryByIdAsync(int id);
    Task<Category> CreateCategoryAsync(Category category);
    Task<Category> UpdateCategoryAsync(int id, Category category);
    Task DeleteCategoryAsync(int id);
}
