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
        // DepartmentsController.cs
        public IActionResult Edit(int id)
        {
            var department = _context.Departments.Find(id);
            if (department == null)
            {
                return NotFound();
            }

            return View(department); // Pass the department object to the Edit view
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(Department department)
        {
            if (ModelState.IsValid)
            {
                _context.Departments.Update(department);
                _context.SaveChanges();

                return RedirectToAction(nameof(Index)); // Redirect back to the list
            }

            return View(department); // If validation fails, return to the Edit view
        }

    }
}
