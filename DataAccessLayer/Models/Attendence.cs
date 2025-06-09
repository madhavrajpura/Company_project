using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DataAccessLayer.Models;

public class Attendence
{
    [Key]
    public int Id { get; set; }
    public DateTime Date { get; set; }
    public DateTime CheckInTime { get; set; }
    public DateTime CheckOutTime { get; set; }
    public bool IsDeleted { get; set; } = false;
    
    [Required]
    [ForeignKey("Employee")]
    public int EmployeeId { get; set; }
    public virtual Employee Employee { get; set; } = null!;

}
