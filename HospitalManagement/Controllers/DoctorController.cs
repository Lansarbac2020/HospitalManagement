using HospitalManagement.Data;
using HospitalManagement.Models;
using Microsoft.AspNetCore.Mvc;
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
            var doctors = await _context.Doctors.Include(d => d.DepartmentHead).ToListAsync();
            return View(doctors);
        }

        // GET: Doctor/Create
        public IActionResult CreateDoctor()
        {
            ViewBag.DepartmentHeads = _context.FacultyMembers.ToList(); // Liste des chefs de départements
            return View();
        }

        // POST: Doctor/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateDoctor(Doctor doctor)
        {
            if (ModelState.IsValid)
            {
                _context.Add(doctor);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewBag.DepartmentHeads = _context.FacultyMembers.ToList();
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

            // Fetch the list of department heads
            var departmentHeads = _context.FacultyMembers.ToList();

            // Pass the department heads list to the view
            ViewData["DepartmentHeads"] = departmentHeads;

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
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!_context.Doctors.Any(e => e.DoctorId == doctor.DoctorId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
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
                .Include(d => d.DepartmentHead) // Include related DepartmentHead details
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
