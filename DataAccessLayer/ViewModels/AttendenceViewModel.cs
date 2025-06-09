using System.ComponentModel.DataAnnotations;

namespace DataAccessLayer.ViewModels;

public class AttendanceViewModel
{
    public int AttendanceId { get; set; }
    public int EmployeeId { get; set; }

    [Required(ErrorMessage = "Date is required")]
    public DateTime Date { get; set; }

    [Required(ErrorMessage = "Check-in time is required")]
    public DateTime CheckInTime { get; set; }

    public DateTime? CheckOutTime { get; set; }
}