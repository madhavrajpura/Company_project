using DataAccessLayer.ViewModels;

namespace BusinessLogicLayer.Services.Interfaces;

public interface IEmployeeService
{
    PaginationViewModel<EmployeeViewModel> GetEmployeeList(int pageNumber = 1, string search = "", int pageSize = 3,string sortColumn = "", string sortDirection = "");
    EmployeeViewModel GetEmployeeById(int id);
    bool SaveEmployee(EmployeeViewModel employeeVM);
    bool DeleteEmployee(int employeeid);
}
