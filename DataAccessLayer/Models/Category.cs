using System.ComponentModel.DataAnnotations;

namespace DataAccessLayer.Models;

public class Category
{
    [Key]
    public int Id { get; set; }

    [Required]
    public string Name { get; set; }
    public bool IsActive { get; set; } = true;
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    public virtual ICollection<Product> Products { get; set; } = new List<Product>();
}