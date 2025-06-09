using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using DataAccessLayer.Models;
using DataAccessLayer.ViewModels;
using BusinessLogicLayer.Services.Interfaces;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Employee_Attendence_Management_System.Controllers;

public class HomeController : Controller
{
    private readonly IEmployeeService _employeeService;

    public HomeController(IEmployeeService employeeService)
    {
        _employeeService = employeeService;
    }

    public IActionResult Index()
    {
        return View();
    }
    public IActionResult GetEmployeeList(int pageNumber = 1, string search = "", int pageSize = 3, string sortColumn = "", string sortDirection = "")
    {
        PaginationViewModel<EmployeeViewModel>? EmployeeList = _employeeService.GetEmployeeList(pageNumber, search, pageSize,sortColumn,sortDirection);
        return PartialView("_EmployeeListPartial", EmployeeList);
    }

    [HttpGet]
    public IActionResult SaveEmployee(int employeeid)
    {

        EmployeeViewModel EmployeeVM = new EmployeeViewModel();

        if (employeeid == 0)
        {
            EmployeeVM = new EmployeeViewModel();
        }
        else
        {
            EmployeeVM = _employeeService.GetEmployeeById(employeeid);
        }

        return PartialView("_SaveEmployeePartial", EmployeeVM);
    }

    [HttpPost]
    public IActionResult SaveEmployee([FromForm] EmployeeViewModel EmployeeVM)
    {
        bool UpdateStatus = _employeeService.SaveEmployee(EmployeeVM);
        return Json(UpdateStatus
            ? new { success = true, text = EmployeeVM.EmployeeId == 0 ? "Employee Added successfully" : "Employee Updated successfully" }
            : new { success = false, text = $"Failed to {(EmployeeVM.EmployeeId == 0 ? "Add" : "Update")} Employee, Check If already exists!" });
    }

    [HttpPost]
    public IActionResult DeleteEmployee(int employeeid)
    {
        bool deleteStatus = _employeeService.DeleteEmployee(employeeid);

        if (deleteStatus)
        {
            return Json(new { success = true, text = "Employee Deleted successfully" });
        }
        return Json(new { success = false, text = "Failed to delete Employee.Try again!" });
    }

}
