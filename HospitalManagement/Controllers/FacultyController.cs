using Microsoft.AspNetCore.Mvc;

namespace HospitalManagement.Controllers
{
    public class FacultyController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
