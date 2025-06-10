using Microsoft.AspNetCore.Mvc;
using DataAccessLayer.ViewModels;
using BusinessLogicLayer.Services.Interfaces;
using Microsoft.AspNetCore.Mvc.Rendering;
using BusinessLogicLayer.Helper;
using Microsoft.AspNetCore.Authorization;

namespace Employee_Attendence_Management_System.Controllers;

public class HomeController : Controller
{
    private readonly IEmployeeService _employeeService;

    public HomeController(IEmployeeService employeeService)
    {
        _employeeService = employeeService;
    }

    [HttpGet]
    public IActionResult Homes()
    {
        return View();
    }

    #region Employee CRUD

    [HttpGet]
    public IActionResult GetEmployeeList(int pageNumber = 1, string search = "", int pageSize = 3, string sortColumn = "", string sortDirection = "")
    {
        PaginationViewModel<EmployeeViewModel>? EmployeeList = _employeeService.GetEmployeeList(pageNumber, search, pageSize, sortColumn, sortDirection);
        Dictionary<int, bool> attendanceStatus  = _employeeService.GetAttendanceStatusForEmployees(EmployeeList.Items.Select(e => e.EmployeeId).ToList(), DateTime.Today);
        ViewBag.AttendanceStatus = attendanceStatus;
        return PartialView("_EmployeeListPartial", EmployeeList);
    }

    [HttpGet]
    public IActionResult SaveEmployee(int employeeid)
    {
        EmployeeViewModel EmployeeVM = (employeeid == 0) ? new EmployeeViewModel() : _employeeService.GetEmployeeById(employeeid);
        return PartialView("_SaveEmployeePartial", EmployeeVM);
    }

    [HttpPost]
    public IActionResult SaveEmployee([FromForm] EmployeeViewModel EmployeeVM)
    {
        if (_employeeService.CheckExists(EmployeeVM))
        {
            return Json(new { success = false, text = NotificationMessage.EmailAlreadyExists });
        }

        bool saveStatus = _employeeService.SaveEmployee(EmployeeVM);
        return Json(saveStatus
            ? new { success = true, text = EmployeeVM.EmployeeId == 0 ? NotificationMessage.CreateSuccess.Replace("{0}", "Employee") : NotificationMessage.UpdateSuccess.Replace("{0}", "Employee") }
            : new { success = false, text = EmployeeVM.EmployeeId == 0 ? NotificationMessage.CreateFailure.Replace("{0}", "Employee") : NotificationMessage.UpdateFailure.Replace("{0}", "Employee") });
    }

    [HttpPost]
    public IActionResult DeleteEmployee(int employeeid)
    {
        bool deleteStatus = _employeeService.DeleteEmployee(employeeid);
        return Json(deleteStatus
            ? new { success = true, text = NotificationMessage.DeleteSuccess.Replace("{0}", "Employee") }
            : new { success = false, text = NotificationMessage.DeleteFailure.Replace("{0}", "Employee") });
    }

    #endregion

    #region Attendance CRUD

    [HttpGet]
    public IActionResult GetAttendance(int employeeId, int attendanceId)
    {
        EmployeeAttendanceViewModel model = new EmployeeAttendanceViewModel
        {
            NewAttendance = new AttendanceViewModel { EmployeeId = employeeId, AttendanceId = 0 },
            AttendanceHistory = _employeeService.GetAttendanceHistory(employeeId)
        };

        if (attendanceId > 0)
        {
            var existingAttendance = _employeeService.GetAttendanceByEmployeeIdAndDate(employeeId, attendanceId);
            if (existingAttendance != null)
            {
                model.NewAttendance = existingAttendance.NewAttendance;
                return PartialView("_EditAttendancePartial", model);
            }
        }

        return PartialView("_AddAttendancePartial", model);
    }

    [HttpPost]
    public IActionResult AddAttendance([FromForm] EmployeeAttendanceViewModel MainVM)
    {
        if (_employeeService.CheckAttendanceExists(MainVM))
        {
            return Json(new { success = false, text = NotificationMessage.AttendanceAlreadyExists });
        }

        bool saveStatus = _employeeService.AddAttendance(MainVM);
        return Json(saveStatus
            ? new { success = true, text = NotificationMessage.CreateSuccess.Replace("{0}", "Attendance") }
            : new { success = false, text = NotificationMessage.CreateFailure.Replace("{0}", "Attendance") });
    }

    [HttpPost]
    public IActionResult UpdateAttendance([FromForm] EmployeeAttendanceViewModel MainVM)
    {
        if (_employeeService.CheckAttendanceExists(MainVM))
        {
            return Json(new { success = false, text = NotificationMessage.AttendanceAlreadyExists });
        }

        bool saveStatus = _employeeService.UpdateAttendance(MainVM);
        return Json(saveStatus
            ? new { success = true, text = NotificationMessage.UpdateSuccess.Replace("{0}", "Attendance") }
            : new { success = false, text = NotificationMessage.UpdateFailure.Replace("{0}", "Attendance") });
    }

    [HttpPost]
    public IActionResult DeleteAttendance(int employeeId, int attendanceId)
    {
        bool deleteStatus = _employeeService.DeleteAttendance(employeeId, attendanceId);
        return Json(deleteStatus
            ? new { success = true, text = NotificationMessage.DeleteSuccess.Replace("{0}", "Attendance") }
            : new { success = false, text = NotificationMessage.DeleteFailure.Replace("{0}", "Attendance") });
    }

    #endregion

    #region Attendance Report

    [HttpGet]
    public IActionResult AttendanceReport()
    {
        List<SelectListItem>? employees = _employeeService.GetAllEmployees()
            .Select(e => new SelectListItem
            {
                Value = e.EmployeeId.ToString(),
                Text = $"{e.FirstName} {e.LastName}"
            })
            .ToList();

        ViewBag.Employees = employees ?? new List<SelectListItem>();
        return View();
    }

    [HttpPost]
    public IActionResult GetAttendanceReport(int employeeId, DateTime startDate, DateTime endDate)
    {
        List<AttendanceViewModel>? reportData = _employeeService.GetAttendanceReport(employeeId, startDate, endDate);
        return PartialView("_AttendanceReportPartial", reportData);
    }

    #endregion

    public IActionResult HandleErrorWithToaster(string message)
    {
        TempData["ErrorMessage"] = message;
        string referer = Request.Headers["Referer"].ToString();
        return Redirect(string.IsNullOrEmpty(referer) ? Url.Action("Homes", "Home") ?? "/" : referer);
    }
}
