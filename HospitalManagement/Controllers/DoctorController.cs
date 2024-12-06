using HospitalManagement.Data;
using HospitalManagement.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace HospitalManagement.Controllers
{
    public class DoctorController : Controller
    {
        private readonly ApplicationDbContext _context;

        public DoctorController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Doctor
        public async Task<IActionResult> Index()
        {
            var doctors = await _context.Doctors.Include(d => d.Department)
               
                .ToListAsync();
            return View(doctors);
        }

        // GET: Doctor/Create
        public IActionResult CreateDoctor()
        {
            ViewBag.Departments = _context.Departments.ToList();
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateDoctor(Doctor doctor)
        {
            if (!ModelState.IsValid)
            {
                foreach (var error in ModelState.Values.SelectMany(v => v.Errors))
                {
                    Console.WriteLine(error.ErrorMessage);
                }
                ViewBag.Departments = _context.Departments.ToList();
                return View(doctor);
            }
            // Check if ModelState is valid before proceeding
            if (ModelState.IsValid)
            {
                try
                {
                    _context.Add(doctor); // Add the doctor to the context
                    await _context.SaveChangesAsync(); // Save the changes
                    return RedirectToAction(nameof(Index)); // Redirect to Index or another view
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error: {ex.Message}"); // Log the error for troubleshooting
                    ModelState.AddModelError("", "An error occurred while saving the doctor.");
                }
            }
            // In case of error or invalid data, re-populate the departments and return the view
            ViewBag.Departments = _context.Departments.ToList();
            return View(doctor);

        }
        // GET: Doctor/Edit/5
        public IActionResult EditDoctor(int id)
        {
            var doctor = _context.Doctors.Find(id);
            if (doctor == null)
            {
                return NotFound();
            }

            // Fetch departments and populate SelectList
            var departments = _context.Departments.ToList();
            if (!departments.Any())
            {
                throw new InvalidOperationException("No departments found in the database.");
            }

            ViewBag.Departments = new SelectList(departments, "DepartmentId", "DepartmentName", doctor.DepartmentId);

            return View(doctor);
        }




        // POST: Doctor/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult EditDoctor(int id, Doctor doctor)
        {
            if (id != doctor.DoctorId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(doctor);
                    _context.SaveChanges();
                    return RedirectToAction(nameof(Index));
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!_context.Doctors.Any(d => d.DoctorId == id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
            }

            // Re-populate the ViewBag.Departments for the dropdown
            var departments = _context.Departments.ToList();
            ViewBag.Departments = new SelectList(departments, "DepartmentId", "DepartmentName", doctor.DepartmentId);

            return View(doctor);
        }




        // GET: Doctor/Delete/5
        public IActionResult DeleteDoctor(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }

            var doctor = _context.Doctors
                .Include(d => d.Department) // Include related DepartmentHead details
                .FirstOrDefault(d => d.DoctorId == id);

            if (doctor == null)
            {
                return NotFound();
            }

            return View(doctor);
        }

        // POST: Doctor/DeleteConfirmed
        [HttpPost, ActionName("DeleteConfirmed")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            var doctorFromDb = _context.Doctors.Find(id);
            if (doctorFromDb == null)
            {
                return NotFound();
            }

            _context.Doctors.Remove(doctorFromDb);
            _context.SaveChanges();

            return RedirectToAction(nameof(Index));
        }
    }
}
