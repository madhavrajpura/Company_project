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

    public IActionResult Home()
    {
        return View();
    }
    public IActionResult GetEmployeeList(int pageNumber = 1, string search = "", int pageSize = 3, string sortColumn = "", string sortDirection = "")
    {
        PaginationViewModel<EmployeeViewModel>? EmployeeList = _employeeService.GetEmployeeList(pageNumber, search, pageSize, sortColumn, sortDirection);
        Dictionary<int, bool> attendanceStatus = _employeeService.GetAttendanceStatusForEmployees(EmployeeList.Items.Select(e => e.EmployeeId).ToList(), DateTime.Today);
        ViewBag.AttendanceStatus = attendanceStatus;
        // PaginationViewModel<EmployeeViewModel>? EmployeeList = _employeeService.GetEmployeeList(pageNumber, search, pageSize, sortColumn, sortDirection);
        // return PartialView("_EmployeeListPartial", EmployeeList);
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

    // NEW FROM HERE --------------------------------------------------------

    [HttpGet]
    public IActionResult GetAttendance(int employeeId, int attendanceId)
    {
        AttendanceViewModel attendanceVM = new AttendanceViewModel();

        if (attendanceId == 0)
        {
            attendanceVM = new AttendanceViewModel();
        }
        else
        {
            attendanceVM = _employeeService.GetAttendanceByEmployeeIdAndDate(employeeId, attendanceId, DateTime.Today);
        }
        return PartialView("_AttendancePartial", attendanceVM);
    }

    [HttpPost]
    public IActionResult SaveAttendance([FromForm] AttendanceViewModel attendanceVM)
    {
        bool saveStatus = _employeeService.SaveAttendance(attendanceVM);
        return Json(saveStatus
            ? new { success = true, text = "Attendance saved successfully" }
            : new { success = false, text = "Failed to save attendance. Check if already exists!" });
    }

    [HttpPost]
    public IActionResult DeleteAttendance(int employeeId)
    {
        bool deleteStatus = _employeeService.DeleteAttendance(employeeId, DateTime.Today);
        return Json(deleteStatus
            ? new { success = true, text = "Attendance deleted successfully" }
            : new { success = false, text = "Failed to delete attendance." });
    }

    // Add this new action for the attendance report
    public IActionResult AttendanceReport()
    {
        // Get all active employees and convert to SelectList
        var employees = _employeeService.GetAllEmployees()
            .Select(e => new SelectListItem
            {
                Value = e.EmployeeId.ToString(),
                Text = $"{e.FirstName} {e.LastName}"
            })
            .ToList();

        // Make sure to pass an empty list if no employees found
        ViewBag.Employees = employees ?? new List<SelectListItem>();

        return View();
    }


    [HttpPost]
    public IActionResult GetAttendanceReport(int employeeId, DateTime startDate, DateTime endDate)
    {
        var reportData = _employeeService.GetAttendanceReport(employeeId, startDate, endDate);
        return PartialView("_AttendanceReportPartial", reportData);
    }

}
