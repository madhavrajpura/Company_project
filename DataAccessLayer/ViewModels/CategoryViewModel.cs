using System.ComponentModel.DataAnnotations;

namespace DataAccessLayer.ViewModels;

public class CategoryViewModel
{
    public int CategoryId { get; set; }

    [Required(AllowEmptyStrings = false, ErrorMessage = "Category Name is required.")]
    [MaxLength(50, ErrorMessage = "Category Name cannot exceed 50 characters.")]
    public string CategoryName { get; set; }
    public bool IsActive { get; set; }
}
