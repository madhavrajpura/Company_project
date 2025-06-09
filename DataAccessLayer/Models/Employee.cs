using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DataAccessLayer.Models;

public class Employee
{
    [Key]
    public int Id { get; set; }
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    [Required]
    public string Email { get; set; }
    [Required]
    public string Department { get; set; }
    public string? Position { get; set; }
    public bool IsDeleted { get; set; } = false;
    public virtual ICollection<Attendence> Attendences { get; set; } = new List<Attendence>();
}