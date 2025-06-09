using BusinessLogicLayer.Services.Interfaces;
using DataAccessLayer.Repository.Interfaces;
using DataAccessLayer.ViewModels;

namespace BusinessLogicLayer.Services.Implementations;

public class EmployeeService : IEmployeeService
{
    private readonly IEmployeeRepository _employeeRepository;

    public EmployeeService(IEmployeeRepository employeeRepository)
    {
        _employeeRepository = employeeRepository;
    }

    public PaginationViewModel<EmployeeViewModel> GetEmployeeList(int pageNumber = 1, string search = "", int pageSize = 3, string sortColumn = "", string sortDirection = "")
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
        return _employeeRepository.GetEmployeeById(id); ;
    }

    public bool SaveEmployee(EmployeeViewModel employeeVM)
    {
        return _employeeRepository.SaveEmployee(employeeVM);
    }

    public bool DeleteEmployee(int employeeid)
    {
        return _employeeRepository.DeleteEmployee(employeeid);
    }

}
