using DEPI.BLL.DTO;
using DEPI.DAL.Enums;
using DEPI.DAL.Models;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DEPI.BLL.Service.Interfaces
{
    public interface IAdminService
    {
        public Task<List<EmployeeStatusDto>> GetPendingEmployeesAsync();
        public Task<List<EmployeeStatusDto>> ApprovedEmployeeAsync();
        public Task<List<EmployeeStatusDto>> RejectedEmployeeAsync();
        public Task ApproveEmployeeAsync(string Email);
        public Task RejectEmployeeAsync(string Email);
        public Task<IdentityResult> EditEmployeeRoleAndStatusAsync(EditEmployeeRoleAndStatus emp);
        public Task<EditEmployeeRoleAndStatus> GetEmployeeAsync(string Email);
        public Task<IdentityResult> UpdateEmployeeAsync(EditEmployeeDto emp);
        public Task<EditEmployeeDto> GetEmployeeByIdAsync(string id);
    }
}
