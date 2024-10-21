using Microsoft.AspNetCore.Mvc;

namespace HospitalManagement.Controllers
{
    public class EmergencyController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
