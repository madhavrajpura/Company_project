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
              AttendanceId = x.Attendences.Where(t => !t.IsDeleted).Select(t => t.Id).FirstOrDefault(),
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
                AttendanceId = x.Attendences.Where(t => !t.IsDeleted).Select(t => t.Id).FirstOrDefault(),
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
    // NEW FROM HERE ---------------------------------------------------------------------

    public AttendanceViewModel GetAttendanceByEmployeeIdAndDate(int employeeId, int attendanceId, DateTime date)
    {
        AttendanceViewModel? Attendence = _db.Attendences.Where(t => t.EmployeeId == employeeId && t.Id == attendanceId && t.Date.Date == date.Date && !t.IsDeleted)
            .Select(x => new AttendanceViewModel
            {
                EmployeeId = x.EmployeeId,
                AttendanceId = x.Id,
                Date = x.Date,
                CheckInTime = x.CheckInTime,
                CheckOutTime = x.CheckOutTime,
            }).AsQueryable().FirstOrDefault();

        if (Attendence == null)
        {
            return null!;
        }
        return Attendence;
    }

    public bool SaveAttendance(AttendanceViewModel attendanceVM)
    {
        if (attendanceVM.AttendanceId == 0)
        {
            Attendence attendence = new Attendence
            {
                CheckInTime = attendanceVM.CheckInTime,
                CheckOutTime = attendanceVM.CheckOutTime,
                Date = attendanceVM.Date,
                EmployeeId = attendanceVM.EmployeeId,
                IsDeleted = false
            };
            _db.Attendences.Add(attendence);
        }
        else
        {
            Attendence? Existattendence = _db.Attendences.FirstOrDefault(x => x.Id == attendanceVM.AttendanceId && x.EmployeeId == attendanceVM.EmployeeId && !x.IsDeleted);

            if (Existattendence == null)
            {
                return false;
            }
            Existattendence.CheckInTime = attendanceVM.CheckInTime;
            Existattendence.CheckOutTime = attendanceVM.CheckOutTime;
            Existattendence.Date = attendanceVM.Date;
            Existattendence.EmployeeId = attendanceVM.EmployeeId;
            _db.Attendences.Update(Existattendence);
        }

        _db.SaveChanges();
        return true;
    }

    public bool DeleteAttendance(int employeeId, DateTime date)
    {
        var attendance = _db.Attendences
            .FirstOrDefault(a => a.EmployeeId == employeeId && a.Date.Date == date.Date && !a.IsDeleted);

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

    // Add these methods to your repository
    public List<EmployeeViewModel> GetAllEmployees()
    {
        return _db.Employees
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
            .ToList() ?? new List<EmployeeViewModel>();
    }

    public List<AttendanceViewModel> GetAttendanceReport(int employeeId, DateTime startDate, DateTime endDate)
    {
        var query = _db.Attendences
            .Where(a => !a.IsDeleted &&
                       a.Date >= startDate &&
                       a.Date <= endDate);

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

}