using HospitalManagement.Data;
using HospitalManagement.Models;
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
        public IActionResult Edit(int? id)
        {
           // var department = _context.Departments.Find(id);
            if (id == null)
            {
                return NotFound();
            }
            Department departmentFromDb = _context.Departments.Find(id);
            if(departmentFromDb==null)
            {
                return NotFound();
            }
            return View(departmentFromDb);
        }



        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit([Bind("DepartmentId,DepartmentName,PatientCount,AvailableBeds")] Department department)
        {
            if (ModelState.IsValid)
            {
                _context.Departments.Update(department);
                _context.SaveChanges();
                return RedirectToAction("Index");
            }

            // Log validation errors
            foreach (var error in ModelState.Values.SelectMany(v => v.Errors))
            {
                Console.WriteLine(error.ErrorMessage); // Or log these errors appropriately
            }

            return View(department);
        }

        // GET: Delete Department
        public IActionResult DeleteDepartment(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }

            // Fetch the department from the database
            var departmentFromDb = _context.Departments.FirstOrDefault(d => d.DepartmentId == id);

            if (departmentFromDb == null)
            {
                return NotFound();
            }

            return View(departmentFromDb); // Pass the department to the view for confirmation
        }


        [HttpPost, ActionName("DeleteConfirmed")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            // Fetch the department to delete
            var departmentFromDb = _context.Departments.Find(id);

            if (departmentFromDb == null)
            {
                return NotFound();
            }

            // Remove the department and save changes
            _context.Departments.Remove(departmentFromDb);
            _context.SaveChanges();

            return RedirectToAction("Index"); // Redirect to the list after deletion
        }


    }
}
