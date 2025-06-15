using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DataAccessLayer.Models;

public class Product
{
    [Key]
    public int Id { get; set; }
    
    [Required]
    public string Name { get; set; }
    public decimal Price { get; set; }

    [Required]
    [ForeignKey("Category")]
    public int CategoryId { get; set; }
    public string? Details { get; set; }
    public bool IsActive { get; set; } = true;
    public string? ImageUrl { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    public virtual Category Category { get; set; } = null!;

}
