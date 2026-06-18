using DEPI.BLL.DTO;
using DEPI.BLL.Service.Interfaces;
using DEPI.DAL.Enums;
using DEPI.DAL.Model;
using DEPI.DAL.Models;
using DEPI.DAL.Repo.Implementation;
using DEPI.DAL.Repo.Interfaces;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DEPI.BLL.Service.Implementation
{
    public class AdminService : IAdminService
    {
        private readonly IEmployeeRepo _employeeRepo;
        private readonly IUserRepo _userRepo;

        public AdminService(IEmployeeRepo employeeRepo, IUserRepo userRepo)
        {
            _employeeRepo = employeeRepo;
            _userRepo = userRepo;
        }
        public async Task<List<EmployeeStatusDto>> ApprovedEmployeeAsync()
        {
            var employees = await _userRepo.GetAllUsersAsync();
            if (employees == null)
            {
                throw new InvalidOperationException("No employees found.");
            }
            var approvedEmployees = employees.Where(p => p.Status.ToString() == "Approved").ToList();
            if (approvedEmployees == null)
            {
                throw new InvalidOperationException("No approved employees found.");
            }
            var emps = approvedEmployees.Select(e => new EmployeeStatusDto
            {
                Id = e.Employee.EmployeeSsn,
                Email = e.Email ?? string.Empty,
                Name = e.Employee.FirstName + " " + e.Employee.LastName,
                Status = e.Status.ToString()
            }).ToList();
            return emps;
        }


        public async Task<List<EmployeeStatusDto>> GetPendingEmployeesAsync()
        {
            var employees = await _userRepo.GetAllUsersAsync();
            if (employees == null)
            {
                throw new InvalidOperationException("No employees found.");
            }
            var pendingEmployees = employees.Where(p => p.Status.ToString() == "Pending").ToList();
            if (pendingEmployees == null)
            {
                throw new InvalidOperationException("No pending employees found.");
            }
            var emps = pendingEmployees.Select(e => new EmployeeStatusDto
            {
                Id = e.Employee.EmployeeSsn,
                Email = e.Email ?? string.Empty,
                Name = e.Employee.FirstName + " " + e.Employee.LastName,
                Status = e.Status.ToString()
            }).ToList();
            return emps;
        }

        public async Task<List<EmployeeStatusDto>> RejectedEmployeeAsync()
        {
            var employees = await _userRepo.GetAllUsersAsync();
            if (employees == null)
            {
                throw new InvalidOperationException("No employees found.");
            }
            var rejectedEmployees = employees.Where(p => p.Status.ToString() == "Rejected").ToList();
            if (rejectedEmployees == null)
            {
                throw new InvalidOperationException("No rejected employees found.");
            }
            var emps = rejectedEmployees.Select(e => new EmployeeStatusDto
            {
                Id = e.Employee.EmployeeSsn,
                Email = e.Email ?? string.Empty,
                Name = e.Employee.FirstName + " " + e.Employee.LastName,
                Status = e.Status.ToString()
            }).ToList();
            return emps;
        }

        public async Task ApproveEmployeeAsync(string Email)
        {
            var user = await _userRepo.GetByEmailAsync(Email);
            if (user == null)
            {
                throw new InvalidOperationException("Employee not found.");
            }
            user.Status = EmployeeStatus.Approved;
        }
        public async Task RejectEmployeeAsync(string Email)
        {
            var user = await _userRepo.GetByEmailAsync(Email);
            if (user == null)
            {
                throw new InvalidOperationException("Employee not found.");
            }
            user.Status = EmployeeStatus.Rejected;
        }

        public async Task<IdentityResult> EditEmployeeRoleAndStatusAsync(EditEmployeeRoleAndStatus emp)
        {
            if (emp != null)
            {
                var user = await _userRepo.GetByEmailAsync(emp.Email);
                var newStatus = Enum.TryParse<EmployeeStatus>(emp.Status, out var status) ? status : user.Status;
                if (newStatus == EmployeeStatus.Approved)
                {
                    await _userRepo.AddToRoleAsync(user, emp.Role);
                    await _userRepo.ChangeUserStatusAsync(user, newStatus);
                    return IdentityResult.Success;
                }
                await _userRepo.ChangeUserStatusAsync(user, newStatus);
                return IdentityResult.Success;
            }
            return IdentityResult.Failed();
        }

        public async Task<EditEmployeeRoleAndStatus> GetEmployeeAsync(string Email)
        {
            var user = await _userRepo.GetAllUsersAsync();
            var employee = user.FirstOrDefault(u => u.Email == Email);
            if (employee == null)
            {
                throw new InvalidOperationException("Employee not found.");
            }
            var roles = await _userRepo.GetRolesAsync(employee);
            return new EditEmployeeRoleAndStatus
            {

                Email = employee.Email,
                Status = employee.Status.ToString(),
                Role = roles.FirstOrDefault() ?? "No Role"
            };
        }

        public async Task<IdentityResult> UpdateEmployeeAsync(EditEmployeeDto emp)
        {
            Employee newemp = new Employee
            {
                Salary = emp.Salary,
                VacationBalance = emp.VacationBalance,
                DefaultRole = emp.DefaultRole,
                ManagerSsn = emp.ManagerSsn,
                ShiftId = emp.ShiftId,
                ProductionLineId = emp.ProductionLineId
            };
            var result = await _employeeRepo.UpdateEmployeeAsync(emp.Ssn, newemp);
            if (result.Succeeded)
            {
                var employee = await _employeeRepo.GetEmployeeById(emp.Ssn);
                if (employee == null)
                {
                    throw new InvalidOperationException("Employee not found after update.");
                }
                var user = await _userRepo.GetUserByIdAsync(employee.UserId);
                if (user == null)
                {
                    throw new InvalidOperationException("Employee not found after update.");
                }
                var userResult = await _userRepo.UpdateUserRoleAsync(user, emp.Role);
                if (userResult.Succeeded)
                {
                    return IdentityResult.Success;
                }
            }
            return IdentityResult.Failed();
        }

        public async Task<EditEmployeeDto> GetEmployeeByIdAsync(string id)
        {
            var employee = await _employeeRepo.GetEmployeeById(id);
            if (employee == null)
            {
                throw new InvalidOperationException("Employee not found.");
            }
            var newemp = new EditEmployeeDto
            {
                Ssn = employee.EmployeeSsn,
                Salary = employee.Salary,
                VacationBalance = employee.VacationBalance,
                DefaultRole = employee.DefaultRole,
                ManagerSsn = employee.ManagerSsn,
                ShiftId = employee.ShiftId,
                ProductionLineId = employee.ProductionLineId
            };
            var user = await _userRepo.GetUserByIdAsync(employee.UserId);
            if (user != null)
            {
                var roles = await _userRepo.GetRolesAsync(user);
                newemp.Role = roles.FirstOrDefault() ?? "No Role";
            }
            return newemp;
        }
    }
}
