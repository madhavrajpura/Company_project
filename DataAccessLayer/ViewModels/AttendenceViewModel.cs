using System.ComponentModel.DataAnnotations;

namespace DataAccessLayer.ViewModels;

public class AttendanceViewModel
{
    public int AttendanceId { get; set; }
    public int EmployeeId { get; set; }
    public string EmployeeName { get; set; }

    [Required(ErrorMessage = "Date is required")]
    public DateTime Date { get; set; }

    [Required(ErrorMessage = "Check-in time is required")]
    public DateTime CheckInTime { get; set; }

    [Required(ErrorMessage = "Check-out time is required")]
    public DateTime CheckOutTime { get; set; }
}