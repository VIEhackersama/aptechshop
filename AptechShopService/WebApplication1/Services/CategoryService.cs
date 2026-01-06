using Microsoft.EntityFrameworkCore;
using WebApplication1.Models;

namespace WebApplication1.Services;

public class CategoryService : ICategoryService
{
    private readonly AppDbContext _context;

    public CategoryService(AppDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Category>> GetAllCategoriesAsync()
    {
        return await _context.Categories.Include(c => c.SubCategories).ToListAsync();
    }

    public async Task<Category> GetCategoryByIdAsync(int id)
    {
         var category = await _context.Categories
            .Include(c => c.SubCategories)
            .FirstOrDefaultAsync(c => c.Id == id);
            
         if (category == null) throw new Exception("Category not found");
         return category;
    }

    public async Task<Category> CreateCategoryAsync(Category category)
    {
        _context.Categories.Add(category);
        await _context.SaveChangesAsync();
        return category;
    }

    public async Task<Category> UpdateCategoryAsync(int id, Category category)
    {
        var existing = await _context.Categories.FindAsync(id);
        if (existing == null) throw new Exception("Category not found");

        existing.Name = category.Name;
        existing.Slug = category.Slug;
        existing.ParentId = category.ParentId;

        await _context.SaveChangesAsync();
        return existing;
    }

    public async Task DeleteCategoryAsync(int id)
    {
        var category = await _context.Categories.FindAsync(id);
        if (category == null) throw new Exception("Category not found");

        _context.Categories.Remove(category);
        await _context.SaveChangesAsync();
    }
}
