namespace DataAccessLayer.ViewModels;

public class EmployeeAttendanceViewModel
{
    public AttendanceViewModel NewAttendance { get; set; }
    public List<AttendanceViewModel> AttendanceHistory { get; set; }
}
