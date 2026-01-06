using System.ComponentModel.DataAnnotations;

namespace WebApplication1.Models;

public abstract class BaseEntity
{
    [Key]
    public int Id { get; set; } 
    public DateTimeOffset CreatedAt { get; set; } = DateTimeOffset.UtcNow;
    public DateTimeOffset UpdatedAt { get; set; } = DateTimeOffset.UtcNow;
}       