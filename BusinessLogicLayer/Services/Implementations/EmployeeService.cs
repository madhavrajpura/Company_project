using DataAccessLayer.Repository.Interfaces;
using DataAccessLayer.ViewModels;
using BusinessLogicLayer.Services.Interfaces;

namespace BusinessLogicLayer.Services.Implementations;

public class EmployeeService : IEmployeeService
{
    private readonly IEmployeeRepository _employeeRepository;

    public EmployeeService(IEmployeeRepository employeeRepository)
    {
        _employeeRepository = employeeRepository;
    }

    public PaginationViewModel<EmployeeViewModel> GetEmployeeList(int pageNumber, string search, int pageSize, string sortColumn, string sortDirection)
    {
        var query = _employeeRepository.GetEmployeeData();

        if (!string.IsNullOrEmpty(search))
        {
            string lowerSearchTerm = search.ToLower();
            query = query.Where(u =>
                u.FirstName.ToLower().Contains(lowerSearchTerm)
                || u.LastName.ToLower().Contains(lowerSearchTerm)
                || u.Email.ToLower().Contains(lowerSearchTerm)
                || u.Department.ToLower().Contains(lowerSearchTerm)
                || u.Position.ToLower().Contains(lowerSearchTerm)
            );
        }

        int totalCount = query.Count();

        switch (sortColumn)
        {
            case "ID":
                query = sortDirection == "asc" ? query.OrderBy(t => t.EmployeeId) : query.OrderByDescending(t => t.EmployeeId);
                break;
            case "Name":
                query = sortDirection == "asc" ? query.OrderBy(t => t.FirstName) : query.OrderByDescending(t => t.FirstName);
                break;
            case "Department":
                query = sortDirection == "asc" ? query.OrderBy(t => t.Department) : query.OrderByDescending(t => t.Department);
                break;
        }

        List<EmployeeViewModel>? items = query.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToList();

        return new PaginationViewModel<EmployeeViewModel>(items, totalCount, pageNumber, pageSize);
    }

    public EmployeeViewModel GetEmployeeById(int id)
    {
        return _employeeRepository.GetEmployeeById(id);
    }

    public bool SaveEmployee(EmployeeViewModel employeeVM)
    {
        return _employeeRepository.SaveEmployee(employeeVM);
    }

    public bool DeleteEmployee(int employeeId)
    {
        return _employeeRepository.DeleteEmployee(employeeId);
    }

    public bool CheckExists(EmployeeViewModel employeeVM)
    {
        return _employeeRepository.CheckExists(employeeVM);
    }

    public EmployeeAttendanceViewModel GetAttendanceByEmployeeIdAndDate(int employeeId, int attendanceId)
    {
        return _employeeRepository.GetAttendanceByEmployeeIdAndDate(employeeId, attendanceId);
    }

    public bool AddAttendance(EmployeeAttendanceViewModel mainVM)
    {
        return _employeeRepository.AddAttendance(mainVM);
    }

    public bool UpdateAttendance(EmployeeAttendanceViewModel mainVM)
    {
        return _employeeRepository.UpdateAttendance(mainVM);
    }

    public bool DeleteAttendance(int employeeId, int attendanceId)
    {
        return _employeeRepository.DeleteAttendance(employeeId, attendanceId);
    }

    public Dictionary<int, bool> GetAttendanceStatusForEmployees(List<int> employeeIds, DateTime date)
    {
        return _employeeRepository.GetAttendanceStatusForEmployees(employeeIds, date);
    }

    public List<EmployeeViewModel> GetAllEmployees()
    {
        return _employeeRepository.GetAllEmployees();
    }

    public List<AttendanceViewModel> GetAttendanceReport(int employeeId, DateTime startDate, DateTime endDate)
    {
        return _employeeRepository.GetAttendanceReport(employeeId, startDate, endDate);
    }

    public List<AttendanceViewModel> GetAttendanceHistory(int employeeId)
    {
        return _employeeRepository.GetAttendanceHistory(employeeId);
    }

    public bool CheckAttendanceExists(EmployeeAttendanceViewModel mainVM)
    {
        return _employeeRepository.CheckAttendanceExists(mainVM);
    }
}