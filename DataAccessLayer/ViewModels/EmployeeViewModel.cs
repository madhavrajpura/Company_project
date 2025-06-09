using System.ComponentModel.DataAnnotations;

namespace DataAccessLayer.ViewModels;

public class EmployeeViewModel
{
    public int EmployeeId { get; set; }

    [Required(AllowEmptyStrings = false, ErrorMessage = "First Name is required")]
    [RegularExpression(@"^[a-zA-Z]+$", ErrorMessage = "First Name must contain only alphabets")]
    [StringLength(30, ErrorMessage = "First Name cannot exceed 30 characters.")]
    public string? FirstName { get; set; }

    [StringLength(30, ErrorMessage = "Last Name cannot exceed 30 characters.")]
    public string? LastName { get; set; }

    [Required(AllowEmptyStrings = false, ErrorMessage = "Email is required")]
    public string Email { get; set; }

    [Required(ErrorMessage = "Department is required")]
    [StringLength(30, ErrorMessage = "Department cannot exceed 30 characters.")]
    public string Department { get; set; }

    [Required(ErrorMessage = "Position is required")]
    [StringLength(50, ErrorMessage = "Position cannot exceed 50 characters.")]
    public string? Position { get; set; }
}
