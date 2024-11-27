using HospitalManagement.Data;
using HospitalManagement.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HospitalManagement.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminController : Controller
    {
        private readonly ApplicationDbContext _context;

        public AdminController(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            // Fetch counts
            var dashboardData = new AdminDashboardViewModel
            {
                DepartmentCount = _context.Departments.Count(),
                FacultyCount = _context.FacultyMembers.Count(),
                AssistantCount = _context.Assistants.Count()
            };

            return View(dashboardData);
        }
    }
}