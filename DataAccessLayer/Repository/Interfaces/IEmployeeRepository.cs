using DataAccessLayer.ViewModels;

namespace DataAccessLayer.Repository.Interfaces;

public interface IEmployeeRepository
{
    IQueryable<EmployeeViewModel> GetEmployeeData();
    EmployeeViewModel GetEmployeeById(int id);
    bool SaveEmployee(EmployeeViewModel employeeVM);
    bool DeleteEmployee(int employeeid);
    bool CheckExists(EmployeeViewModel employeeVM);
    // NEWWWWWWWWWWWWWWWWWWWWWW
    AttendanceViewModel GetAttendanceByEmployeeIdAndDate(int employeeId, int attendanceId, DateTime date);
    bool SaveAttendance(AttendanceViewModel attendanceVM);
    bool DeleteAttendance(int employeeId, DateTime date);
    Dictionary<int, bool> GetAttendanceStatusForEmployees(List<int> employeeIds, DateTime date);
    List<EmployeeViewModel> GetAllEmployees();
    List<AttendanceViewModel> GetAttendanceReport(int employeeId, DateTime startDate, DateTime endDate);
}
