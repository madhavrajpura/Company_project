using DataAccessLayer.Models;
using DataAccessLayer.ViewModels;

namespace BusinessLogicLayer.Services.Interfaces;

public interface IEmployeeService
{
    PaginationViewModel<EmployeeViewModel> GetEmployeeList(int pageNumber = 1, string search = "", int pageSize = 3, string sortColumn = "", string sortDirection = "");
    EmployeeViewModel GetEmployeeById(int id);
    bool SaveEmployee(EmployeeViewModel employeeVM);
    bool DeleteEmployee(int employeeid);
    AttendanceViewModel GetAttendanceByEmployeeIdAndDate(int employeeId, int attendanceId, DateTime date);
    bool SaveAttendance(AttendanceViewModel attendanceVM);
    bool DeleteAttendance(int employeeId, DateTime date);
    Dictionary<int, bool> GetAttendanceStatusForEmployees(List<int> employeeIds, DateTime date);
    List<EmployeeViewModel> GetAllEmployees();
    List<AttendanceViewModel> GetAttendanceReport(int employeeId, DateTime startDate, DateTime endDate);
}
