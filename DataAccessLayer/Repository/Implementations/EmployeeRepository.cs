using DataAccessLayer.Models;
using DataAccessLayer.Repository.Interfaces;
using DataAccessLayer.ViewModels;

namespace DataAccessLayer.Repository.Implementations;

public class EmployeeRepository : IEmployeeRepository
{
    private readonly ApplicationDBContext _db;

    public EmployeeRepository(ApplicationDBContext db)
    {
        _db = db;
    }

    public IQueryable<EmployeeViewModel> GetEmployeeData()
    {
        IQueryable<EmployeeViewModel> query = _db.Employees
          .Where(x => !x.IsDeleted)
          .Select(x => new EmployeeViewModel
          {
              EmployeeId = x.Id,
              FirstName = x.FirstName,
              LastName = x.LastName,
              Email = x.Email,
              Department = x.Department,
              Position = x.Position
          })
          .AsQueryable().OrderBy(x => x.EmployeeId);

        return query;
    }

    public EmployeeViewModel GetEmployeeById(int id)
    {
        EmployeeViewModel? employee = _db.Employees.Where(t => t.Id == id && !t.IsDeleted)
            .Select(x => new EmployeeViewModel
            {
                EmployeeId = x.Id,
                FirstName = x.FirstName,
                LastName = x.LastName,
                Email = x.Email,
                Department = x.Department,
                Position = x.Position
            }).AsQueryable().FirstOrDefault();

        if (employee == null)
        {
            return null!;
        }
        return employee;
    }

    public bool SaveEmployee(EmployeeViewModel employeeVM)
    {
        if (employeeVM.EmployeeId == 0)
        {
            Employee employee = new Employee
            {
                FirstName = employeeVM.FirstName,
                LastName = employeeVM.LastName,
                Email = employeeVM.Email,
                Department = employeeVM.Department,
                Position = employeeVM.Position,
                IsDeleted = false
            };
            _db.Employees.Add(employee);
        }
        else
        {
            Employee? ExistEmployee = _db.Employees.FirstOrDefault(x => x.Id == employeeVM.EmployeeId && !x.IsDeleted);

            if (ExistEmployee == null)
            {
                return false;
            }
            ExistEmployee.FirstName = employeeVM.FirstName;
            ExistEmployee.LastName = employeeVM.LastName;
            ExistEmployee.Email = employeeVM.Email;
            ExistEmployee.Department = employeeVM.Department;
            ExistEmployee.Position = employeeVM.Position;
            _db.Employees.Update(ExistEmployee);
        }

        _db.SaveChanges();
        return true;
    }

    public bool DeleteEmployee(int employeeid)
    {
        Employee? employee = _db.Employees.FirstOrDefault(t => t.Id == employeeid && !t.IsDeleted);
        if (employee != null)
        {
            employee.IsDeleted = true;
            _db.SaveChanges();
            return true;
        }
        return false;
    }

    public bool CheckExists(EmployeeViewModel employeeVM)
    {
        if (employeeVM.EmployeeId == 0)
        {
            return _db.Employees.Any(x => x.Email.ToLower().Trim() == employeeVM.Email.ToLower().Trim() && !x.IsDeleted);
        }
        else
        {
            return _db.Employees.Any(x => x.Id != employeeVM.EmployeeId && x.Email.ToLower().Trim() == employeeVM.Email.ToLower().Trim() && !x.IsDeleted);
        }
    }

}