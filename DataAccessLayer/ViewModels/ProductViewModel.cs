using System.ComponentModel.DataAnnotations;

using Microsoft.AspNetCore.Http;

namespace DataAccessLayer.ViewModels;

public class ProductViewModel
{
    public int ProductId { get; set; }

    [Required(AllowEmptyStrings = false, ErrorMessage = "Product Name is required.")]
    [MaxLength(100, ErrorMessage = "Product Name cannot exceed 100 characters.")]
    public string ProductName { get; set; }

    [Required(ErrorMessage = "Price is required.")]
    [Range(0.01, double.MaxValue, ErrorMessage = "Price must be greater than zero.")]
    public decimal Price { get; set; }

    public string? Details { get; set; }
    public bool IsActive { get; set; }
    public string? ImageUrl { get; set; }
    public IFormFile? ImageFile { get; set; }
    public int CategoryId { get; set; }
    public string? CategoryName { get; set; }
}
