using HospitalManagement.Data;
using HospitalManagement.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace HospitalManagement.Controllers
{
    public class FacultyController : Controller
    {
        private readonly ApplicationDbContext _context;

        public FacultyController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Faculty
        public async Task<IActionResult> Index()
        {
            var facultyMembers = await _context.FacultyMembers.Include(f => f.Department).ToListAsync();
            return View(facultyMembers);
        }

        // GET: Faculty/Create
        public IActionResult CreateFacultyMember()
        {
            ViewBag.Departments = _context.Departments.ToList();
            return View();
        }

        // POST: Faculty/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateFacultyMember(FacultyMember facultyMember)
        {
            if (ModelState.IsValid)
            {
                _context.Add(facultyMember);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewBag.Departments = _context.Departments.ToList();
            return View(facultyMember);
        }

        // GET: Faculty/Edit/5
        // FacultyController.cs
        public IActionResult Edit(int id)
        {
            var facultyMember = _context.FacultyMembers.Find(id);
            if (facultyMember == null)
            {
                return NotFound();
            }

            // Fetch the list of departments
            var departments = _context.Departments.ToList();

            // Pass the departments list to the view
            ViewData["Departments"] = departments;

            return View(facultyMember); // This will pass the facultyMember to the Edit view
        }


        // POST: Faculty/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int id, FacultyMember facultyMember)
        {
            if (id != facultyMember.FacultyId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(facultyMember);
                    _context.SaveChanges();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!_context.FacultyMembers.Any(e => e.FacultyId == facultyMember.FacultyId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index)); // Assuming you have an Index action to return the list
            }
            return View(facultyMember);
        }
        // GET: Faculty/Delete/5
        public IActionResult Delete(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }

            var facultyMember = _context.FacultyMembers
                .Include(f => f.Department) // Include related Department details
                .FirstOrDefault(f => f.FacultyId == id);

            if (facultyMember == null)
            {
                return NotFound();
            }

            return View(facultyMember);
        }

        // POST: Faculty/DeleteConfirmed
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            var facultyMember = _context.FacultyMembers.Find(id);
            if (facultyMember == null)
            {
                return NotFound();
            }

            _context.FacultyMembers.Remove(facultyMember);
            _context.SaveChanges();

            return RedirectToAction(nameof(Index));
        }



    }
}
