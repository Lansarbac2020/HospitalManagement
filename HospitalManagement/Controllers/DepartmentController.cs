using Microsoft.AspNetCore.Mvc;

namespace HospitalManagement.Controllers
{
    public class DepartmentController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
