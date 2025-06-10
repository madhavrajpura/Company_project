using DataAccessLayer.ViewModels;

namespace BusinessLogicLayer.Services.Interfaces;

public interface IEmployeeService
{
    PaginationViewModel<EmployeeViewModel> GetEmployeeList(int pageNumber = 1, string search = "", int pageSize = 3, string sortColumn = "", string sortDirection = "");
    EmployeeViewModel GetEmployeeById(int id);
    bool SaveEmployee(EmployeeViewModel employeeVM);
    bool DeleteEmployee(int employeeid);
    bool CheckExists(EmployeeViewModel employeeVM);
    EmployeeAttendanceViewModel GetAttendanceByEmployeeIdAndDate(int employeeId, int attendanceId);
    bool DeleteAttendance(int employeeId, int attendanceId);
    Dictionary<int, bool> GetAttendanceStatusForEmployees(List<int> employeeIds, DateTime date);
    List<EmployeeViewModel> GetAllEmployees();
    List<AttendanceViewModel> GetAttendanceReport(int employeeId, DateTime startDate, DateTime endDate);
    List<AttendanceViewModel> GetAttendanceHistory(int employeeId);
    bool CheckAttendanceExists(EmployeeAttendanceViewModel MainVM);
    bool AddAttendance(EmployeeAttendanceViewModel mainVM);
    bool UpdateAttendance(EmployeeAttendanceViewModel mainVM);



}
