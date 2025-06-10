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

    #region Employee CRUD

    public IQueryable<EmployeeViewModel> GetEmployeeData()
    {
        return _db.Employees
            .Where(x => !x.IsDeleted)
            .Select(x => new EmployeeViewModel
            {
                EmployeeId = x.Id,
                AttendanceId = x.Attendences.Where(t => !t.IsDeleted).Select(t => t.Id).FirstOrDefault(),
                FirstName = x.FirstName,
                LastName = x.LastName,
                Email = x.Email,
                Department = x.Department,
                Position = x.Position
            })
            .OrderBy(x => x.EmployeeId);
    }

    public EmployeeViewModel GetEmployeeById(int id)
    {
        return _db.Employees
.Where(t => t.Id == id && !t.IsDeleted)
            .Select(x => new EmployeeViewModel
            {
                EmployeeId = x.Id,
                AttendanceId = x.Attendences.Where(t => !t.IsDeleted).Select(t => t.Id).FirstOrDefault(),
                FirstName = x.FirstName,
                LastName = x.LastName,
                Email = x.Email,
                Department = x.Department,
                Position = x.Position
            })
            .FirstOrDefault() ?? new EmployeeViewModel();
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
            if (ExistEmployee == null) return false;

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
        return _db.Employees.Any(x =>
x.Email.ToLower().Trim() == employeeVM.Email.ToLower().Trim() &&
            !x.IsDeleted &&
(employeeVM.EmployeeId == 0 || x.Id != employeeVM.EmployeeId));
    }

    #endregion

    #region Attendance CRUD

    public EmployeeAttendanceViewModel GetAttendanceByEmployeeIdAndDate(int employeeId, int attendanceId)
    {
        var attendance = _db.Attendences
.Where(t => t.EmployeeId == employeeId && t.Id == attendanceId && !t.IsDeleted)
            .Select(x => new EmployeeAttendanceViewModel
            {
                NewAttendance = new AttendanceViewModel
                {
                    AttendanceId = x.Id,
                    EmployeeId = x.EmployeeId,
                    CheckInTime = x.CheckInTime,
                    CheckOutTime = x.CheckOutTime,
                    Date = x.Date
                },
                AttendanceHistory = _db.Attendences
                    .Where(a => a.EmployeeId == employeeId && !a.IsDeleted)
.OrderByDescending(a => a.Date)
                    .Select(a => new AttendanceViewModel
                    {
                        AttendanceId = a.Id,
                        EmployeeId = a.EmployeeId,
                        Date = a.Date,
                        CheckInTime = a.CheckInTime,
                        CheckOutTime = a.CheckOutTime
                    })
                    .ToList()
            })
            .FirstOrDefault() ?? new EmployeeAttendanceViewModel
            {
                NewAttendance = new AttendanceViewModel { EmployeeId = employeeId },
                AttendanceHistory = GetAttendanceHistory(employeeId)
            };

        return attendance;
    }

    public bool AddAttendance(EmployeeAttendanceViewModel MainVM)
    {
        Attendence attendance = new Attendence
        {
            CheckInTime = MainVM.NewAttendance.CheckInTime,
            CheckOutTime = MainVM.NewAttendance.CheckOutTime,
            Date = MainVM.NewAttendance.Date,
            EmployeeId = MainVM.NewAttendance.EmployeeId,
            IsDeleted = false
        };
        _db.Attendences.Add(attendance);
        _db.SaveChanges();
        return true;
    }

    public bool UpdateAttendance(EmployeeAttendanceViewModel MainVM)
    {
        Attendence? existingAttendance = _db.Attendences
.FirstOrDefault(x => x.Id == MainVM.NewAttendance.AttendanceId && x.EmployeeId == MainVM.NewAttendance.EmployeeId && !x.IsDeleted);

        if (existingAttendance == null) return false;

        existingAttendance.CheckInTime = MainVM.NewAttendance.CheckInTime;
        existingAttendance.CheckOutTime = MainVM.NewAttendance.CheckOutTime;
        existingAttendance.Date = MainVM.NewAttendance.Date;
        existingAttendance.EmployeeId = MainVM.NewAttendance.EmployeeId;
        _db.Attendences.Update(existingAttendance);
        _db.SaveChanges();
        return true;
    }

    public bool DeleteAttendance(int employeeId, int attendanceId)
    {
        var attendance = _db.Attendences
.FirstOrDefault(a => a.EmployeeId == employeeId && a.Id == attendanceId && !a.IsDeleted);

        if (attendance != null)
        {
            attendance.IsDeleted = true;
            _db.SaveChanges();
            return true;
        }
        return false;
    }

    public Dictionary<int, bool> GetAttendanceStatusForEmployees(List<int> employeeIds, DateTime date)
    {
        var attendanceRecords = _db.Attendences
.Where(a => employeeIds.Contains(a.EmployeeId) && a.Date.Date == date.Date && !a.IsDeleted)
            .Select(a => a.EmployeeId)
            .ToList();

        return employeeIds.ToDictionary(id => id, id => attendanceRecords.Contains(id));
    }

    #endregion

    #region Attendance Report

    public List<EmployeeViewModel> GetAllEmployees()
    {
        return _db.Employees
            .Where(e => !e.IsDeleted)
            .Select(e => new EmployeeViewModel
            {
                EmployeeId = e.Id,
                FirstName = e.FirstName,
                LastName = e.LastName,
                Email = e.Email,
                Department = e.Department,
                Position = e.Position
            })
            .ToList() ?? new List<EmployeeViewModel>();
    }

    public List<AttendanceViewModel> GetAttendanceReport(int employeeId, DateTime startDate, DateTime endDate)
    {
        var query = _db.Attendences
.Where(a => !a.IsDeleted && a.Date >= startDate && a.Date <= endDate);

        if (employeeId > 0)
        {
            query = query.Where(a => a.EmployeeId == employeeId);
        }

        return query
            .Select(a => new AttendanceViewModel
            {
                AttendanceId = a.Id,
                EmployeeId = a.EmployeeId,
                EmployeeName = a.Employee.FirstName + " " + a.Employee.LastName,
                Date = a.Date,
                CheckInTime = a.CheckInTime,
                CheckOutTime = a.CheckOutTime
            })
            .ToList();
    }

    #endregion

    public List<AttendanceViewModel> GetAttendanceHistory(int employeeId)
    {
        return _db.Attendences
            .Where(a => a.EmployeeId == employeeId && !a.IsDeleted)
.OrderByDescending(a => a.Date)
            .Select(a => new AttendanceViewModel
            {
                AttendanceId = a.Id,
                EmployeeId = a.EmployeeId,
                Date = a.Date,
                CheckInTime = a.CheckInTime,
                CheckOutTime = a.CheckOutTime
            })
            .ToList() ?? new List<AttendanceViewModel>();
    }

    public bool CheckAttendanceExists(EmployeeAttendanceViewModel MainVM)
    {
        return _db.Attendences.Any(x =>
            x.EmployeeId == MainVM.NewAttendance.EmployeeId &&
x.Date.Date == MainVM.NewAttendance.Date.Date &&
            !x.IsDeleted &&
(MainVM.NewAttendance.AttendanceId == 0 || x.Id != MainVM.NewAttendance.AttendanceId));
    }
}