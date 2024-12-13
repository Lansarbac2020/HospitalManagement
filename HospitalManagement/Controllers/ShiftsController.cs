using HospitalManagement.Data;
using HospitalManagement.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace HospitalManagement.Controllers
{

    public class ShiftsController : Controller
    {
        private readonly ApplicationDbContext _db;

        public ShiftsController(ApplicationDbContext db)
        {
            _db = db;
        }

        // GET: Shifts
        public IActionResult Index()
        {
            // Récupérer la liste des shifts avec l'assistant associé, sans charger le département
            var shifts = _db.Shifts
                .Include(s => s.Assistant) 
                .ToList();

            return View(shifts); // Passer la liste des shifts à la vue
        }


        // GET: CreateShift
        [Authorize(Roles = "Admin")]
        public IActionResult CreateShift()
        {
            ViewBag.Assistants = new SelectList(
                _db.Assistants.Select(a => new { a.AssistantId, FullName = $"{a.FirstName} {a.LastName}" }),
                "AssistantId",
                "FullName"
            );

            ViewBag.Departments = new SelectList(
                _db.Departments,
                "DepartmentId",
                "DepartmentName"
            );

            return View(new Shift());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public IActionResult CreateShift(Shift shift)
        {
            if (ModelState.IsValid)
            {
                // Verify the assistant exists
                var assistant = _db.Assistants.FirstOrDefault(a => a.AssistantId == shift.AssistantId);

                if (assistant == null)
                {
                    ModelState.AddModelError("AssistantId", "The selected assistant does not exist.");
                    PopulateAssistantsDropdown(shift);
                    return View(shift);
                }

                // Check for overlapping shifts
                bool isDuplicate = _db.Shifts.Any(s =>
                    s.AssistantId == shift.AssistantId &&
                    s.ShiftDate == shift.ShiftDate &&
                    (
                        (s.StartTime <= shift.StartTime && s.EndTime > shift.StartTime) ||
                        (s.StartTime < shift.EndTime && s.EndTime >= shift.EndTime)
                    ));

                if (isDuplicate)
                {
                    ModelState.AddModelError("", "An overlap with another shift was detected.");
                    PopulateAssistantsDropdown(shift);
                    return View(shift);
                }

                if (shift.StartTime >= shift.EndTime)
                {
                    ModelState.AddModelError("", "Start time must be before end time.");
                    PopulateAssistantsDropdown(shift);
                    return View(shift);
                }

                // Add the shift to the database
                _db.Shifts.Add(shift);
                _db.SaveChanges();
                return RedirectToAction("Index");
            }

            // If we got this far, something failed, return the view with the model
            PopulateAssistantsDropdown(shift);
            return View(shift);
        }

        private void PopulateAssistantsDropdown(Shift shift)
        {
            ViewBag.Assistants = new SelectList(
                _db.Assistants.Select(a => new { a.AssistantId, FullName = $"{a.FirstName} {a.LastName}" }),
                "AssistantId",
                "FullName",
                shift.AssistantId // Pre-select the current assistant
            );

            ViewBag.Departments = new SelectList(
                _db.Departments,
                "DepartmentId",
                "DepartmentName"
            );
        }


        [HttpGet]
        public async Task<JsonResult> GetShifts()
        {
            var currentDate = DateTime.Now;

            var shifts = await _db.Shifts
                .Include(s => s.Assistant)
              
                .Select(s => new
                {
                    id = s.ShiftId,
                    title = $"{s.Assistant.FirstName} {s.Assistant.LastName}",
                    start = s.ShiftDate.Add(s.StartTime),
                    end = s.ShiftDate.Add(s.EndTime),
                    color = s.ShiftDate.Add(s.StartTime) < currentDate ? "red" : "#3788d8", // Change color to red if the shift is in the past
                    departmentName = s.Assistant.Department.DepartmentName
                })
                .ToListAsync();

            return Json(shifts);
        }


    }
}
