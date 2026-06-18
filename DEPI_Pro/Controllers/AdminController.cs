using DEPI.BLL.DTO;
using DEPI.BLL.Service.Interfaces;
using DEPI.DAL.DbContext;
using DEPI.DAL.Model;
using DEPI.DAL.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Specialized;

namespace DEPI.PLL.Controllers
{
    public class AdminController : Controller
    {
        private readonly IAccountService _accountService;
        private readonly IAdminService _adminService;
        public AdminController(IAccountService accountService, IAdminService adminService)
        {
            _accountService = accountService;
            _adminService = adminService;
        }
        [HttpGet]
        public async Task<IActionResult> DisplayEmployees()
        {
            var users = await _adminService.ApprovedEmployeeAsync();
            if (users.Count == 0)
            {
                users = await _adminService.GetPendingEmployeesAsync();
            }
            return View(users);
        }
        [HttpGet]
        public async Task<IActionResult> EditEmployeeStatusAndRole(string email)
        {
            var user = await _adminService.GetEmployeeAsync(email);
            return View(user);
        }
        [HttpPost]
        public async Task<IActionResult> EditEmployeeStatusAndRole(EditEmployeeRoleAndStatus emp)
        {
            var result = await _adminService.EditEmployeeRoleAndStatusAsync(emp);
            if (result.Succeeded)
            {
                return RedirectToAction("DisplayEmployees");
            }
            return View(emp);
        }
        [HttpGet]
        public async Task<IActionResult> PendingEmployees()
        {
            var users = await _adminService.GetPendingEmployeesAsync();
            return Json(users);
        }
        public async Task<IActionResult> RejectedEmployees()
        {
            var users = await _adminService.RejectedEmployeeAsync();
            return Json(users);
        }
        public async Task<IActionResult> ApprovedEmployees()
        {
            var users = await _adminService.ApprovedEmployeeAsync();
            return Json(users);
        }
        [HttpGet]
        public async Task<IActionResult> EditEmployee(string id)
        {
            var emp = await _adminService.GetEmployeeByIdAsync(id);
            return View(emp);
        }
        [HttpPost]
        public async Task<IActionResult> EditEmployee(EditEmployeeDto emp)
        {
            if (ModelState.IsValid)
            {
                var result = await _adminService.UpdateEmployeeAsync(emp);
                if (result.Succeeded)
                {
                    return RedirectToAction("DisplayEmployees");
                }
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError("", error);
                }
            }
            return View(emp);
        }
    }
}
