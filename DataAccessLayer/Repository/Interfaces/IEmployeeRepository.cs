using DataAccessLayer.ViewModels;

namespace DataAccessLayer.Repository.Interfaces;

public interface IEmployeeRepository
{
    IQueryable<EmployeeViewModel> GetEmployeeData();
    EmployeeViewModel GetEmployeeById(int id);
    bool SaveEmployee(EmployeeViewModel employeeVM);
    bool DeleteEmployee(int employeeid);
    bool CheckExists(EmployeeViewModel employeeVM);

}
