using HospitalManagement.Data;
using Microsoft.AspNetCore.Mvc;

namespace HospitalManagement.Controllers
{
    public class DepartmentController : Controller
    {
        private readonly ApplicationDbContext _context;
        public DepartmentController(ApplicationDbContext db)
        {
            _context = db;
        }
        public IActionResult Index()
        {
            List<Department> objDepartmanList=_context.Departments.ToList();
            return View(objDepartmanList);
        }
        public IActionResult CreateDepartmant()
        {
            return View();
        }
        [HttpPost]
        public IActionResult CreateDepartmant(Department obj)
        {
          
            if (ModelState.IsValid)
            {
                _context.Departments.Add(obj);
                _context.SaveChanges();
                return RedirectToAction("Index");
            }
            return View();
        }
       
        
    }
}
