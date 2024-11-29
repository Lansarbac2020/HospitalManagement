using HospitalManagement.Data;
using HospitalManagement.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;


namespace HospitalManagement.Controllers
{
    public class DepartmentController : Controller
    {
        private readonly ApplicationDbContext _context;
        public DepartmentController(ApplicationDbContext db)
        {
            _context = db;
        }
        public async Task<IActionResult> Index()
        {
            var departments = await _context.Departments
                .Include(d => d.FacultyMember)  // Include FacultyMember to access FacultyMember's data
                .ToListAsync();

            return View(departments);
        }

        public IActionResult CreateDepartmant()
        {
            var facultyMembers = _context.FacultyMembers
                .Select(fm => new SelectListItem
                {
                    Value = fm.FacultyId.ToString(),
                    Text = fm.FirstName + " " + fm.LastName
                }).ToList();

            ViewBag.FacultyMembers = facultyMembers;
            return View();
        }


        [HttpPost]
        public IActionResult CreateDepartmant(Department obj)
        {
            if (obj == null)
            {
                return BadRequest("Invalid department data.");
            }

            // Check if the FacultyMemberId is already assigned to another department
            var existingDepartment = _context.Departments
                .FirstOrDefault(d => d.FacultyMemberId == obj.FacultyMemberId);

            if (existingDepartment != null)
            {
                ViewBag.AlertMessage = "This Faculty Member is already assigned to another department.";
            }

            // Always assign FacultyMembers SelectList to ViewBag
            ViewBag.FacultyMembers = new SelectList(_context.FacultyMembers, "FacultyId", "FullName", obj.FacultyMemberId);

            if (ModelState.IsValid && existingDepartment == null)
            {
                // Save department if no errors
                _context.Departments.Add(obj);
                _context.SaveChanges();
                return RedirectToAction("Index");
            }

            // Return the view with errors (if any)
            return View(obj);
        }




        // DepartmentsController.cs
        public IActionResult Edit(int? id)
        {
           // var department = _context.Departments.Find(id);
            if (id == null)
            {
                return NotFound();
            }
            var department = _context.Departments.Include(d => d.FacultyMember).FirstOrDefault(d => d.DepartmentId == id);
            if (department == null) return NotFound();

            ViewBag.FacultyMembers = _context.FacultyMembers.ToList();
            return View(department);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(Department department)
        {
            // Check if the FacultyMemberId is already assigned to another department (excluding the current one)
            var existingDepartment = _context.Departments
                .FirstOrDefault(d => d.FacultyMemberId == department.FacultyMemberId && d.DepartmentId != department.DepartmentId);

            if (existingDepartment != null)
            {
                ModelState.AddModelError("FacultyMemberId", "This Faculty Member is already assigned to another department.");
            }

            if (ModelState.IsValid)
            {
                _context.Departments.Update(department);
                _context.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.FacultyMembers = new SelectList(_context.FacultyMembers, "FacultyId", "FullName", department?.FacultyMemberId);
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
