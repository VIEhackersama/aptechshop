using WebApplication1.Models;

namespace WebApplication1.Services;

using Microsoft.EntityFrameworkCore;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<Customer> Customers { get; set; }
    public DbSet<Category> Categories { get; set; }
    public DbSet<Product> Products { get; set; }
    public DbSet<Order> Orders { get; set; }
    public DbSet<OrderItem> OrderItems { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        // Tự động chuyển CamelCase (C#) sang SnakeCase (Postgres)
        // Ví dụ: FullName -> full_name
        optionsBuilder.UseSnakeCaseNamingConvention();
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Cấu hình OrderItem
        modelBuilder.Entity<OrderItem>(entity =>
        {
            // Map cột TotalPrice (Generated Column) để EF Core biết không được Insert/Update vào cột này
            entity.Property(e => e.TotalPrice)
                .HasComputedColumnSql("quantity * unit_price", stored: true);
        });
        
        // Cấu hình Enum thành String trong DB (Dễ đọc hơn số 0,1,2)
        modelBuilder.Entity<Order>()
            .Property(e => e.Status)
            .HasConversion<string>();

        // Thiết lập quan hệ 1-N nếu cần (thường EF tự hiểu, nhưng khai báo rõ thì tốt hơn)
        modelBuilder.Entity<Product>()
            .HasOne(p => p.Category)
            .WithMany(c => c.Products)
            .HasForeignKey(p => p.CategoryId);
    }
}