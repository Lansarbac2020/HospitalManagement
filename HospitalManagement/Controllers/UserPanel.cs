using HospitalManagement.Data;
using Microsoft.AspNetCore.Mvc;

namespace HospitalManagement.Controllers
{
    public class UserPanelController : Controller
    {
        private readonly ApplicationDbContext _context;

        public UserPanelController(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult Calendar()
        {
            // Pass required data to the view
            ViewBag.Appointments = _context.Appointments.ToList(); // Example for dropdown data
            return View();
        }
    }

}
