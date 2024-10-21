using Microsoft.AspNetCore.Mvc;

namespace HospitalManagement.Controllers
{
    public class AppointmentController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
