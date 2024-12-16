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

                bool isDuplicate = _db.Shifts.Any(s =>
      s.ShiftId != shift.ShiftId && // Exclude the current shift
      s.AssistantId == shift.AssistantId &&
      s.ShiftDate == shift.ShiftDate &&
      (
          (s.StartTime <= shift.StartTime && s.EndTime > shift.StartTime) ||
          (s.StartTime < shift.EndTime && s.EndTime >= shift.EndTime)
      ));
                if (isDuplicate)
                {
                    ModelState.AddModelError("", "An overlap with another shift was detected.");
                    ViewBag.Assistants = new SelectList(
                        _db.Assistants.Select(a => new { a.AssistantId, FullName = $"{a.FirstName} {a.LastName}" }),
                        "AssistantId",
                        "FullName",
                        shift.AssistantId
                    );
                    return View(shift);
                }
                if (shift.StartTime >= shift.EndTime)
                {
                    ModelState.AddModelError("", "Start time must be before end time.");
                    ViewBag.Assistants = new SelectList(
                        _db.Assistants.Select(a => new { a.AssistantId, FullName = $"{a.FirstName} {a.LastName}" }),
                        "AssistantId",
                        "FullName",
                        shift.AssistantId
                    );
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
                .ThenInclude(a => a.Department) // Inclure le département
                .Select(s => new
                {
                    id = s.ShiftId,
                    title = $"{s.Assistant.FirstName} {s.Assistant.LastName}",
                    start = s.ShiftDate.Add(s.StartTime),
                    end = s.ShiftDate.Add(s.EndTime),
                    color = s.ShiftDate.Add(s.StartTime) < currentDate
                        ? "red" // Rouge pour les shifts passés
                        : (s.Assistant.Department.DepartmentName == "Pediatric Emergency" ? "#1E90FF" // Bleu
                        : s.Assistant.Department.DepartmentName == "Pediatric Intensive Care" ? "#32CD32" // Vert
                        : s.Assistant.Department.DepartmentName == "Pediatric Hematology and Oncology" ? "#FFD700" // Jaune
                        : "#3788d8"), // Couleur par défaut
                    departmentName = s.Assistant.Department.DepartmentName
                })
                .ToListAsync();

            return Json(shifts);
        }


        // GET: EditShift
       
        public IActionResult EditShift(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }

            // Fetch the Shift from the database
            Shift shiftFromDb = _db.Shifts
                .Include(s => s.Assistant)  // Include Assistant details if needed
                .FirstOrDefault(s => s.ShiftId == id);

            if (shiftFromDb == null)
            {
                return NotFound();
            }

            // Populate ViewBag with list of Assistants for dropdown
            ViewBag.Assistants = new SelectList(
                _db.Assistants.Select(a => new { a.AssistantId, FullName = $"{a.FirstName} {a.LastName}" }),
                "AssistantId",
                "FullName",
                shiftFromDb.AssistantId // Pre-select the current assistant
            );

            // Return the Shift model to the view
            return View(shiftFromDb);
        }



        // POST: EditShift
        // POST: EditShift
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult EditShift(Shift shift)
        {
            if (ModelState.IsValid)
            {
                // Fetch the Shift from the database using ShiftId
                var shiftFromDb = _db.Shifts.FirstOrDefault(s => s.ShiftId == shift.ShiftId);

                if (shiftFromDb == null)
                {
                    return NotFound();
                }

                // Update properties of shiftFromDb with the new data from shift
                shiftFromDb.AssistantId = shift.AssistantId;
                shiftFromDb.ShiftDate = shift.ShiftDate;
                shiftFromDb.StartTime = shift.StartTime;
                shiftFromDb.EndTime = shift.EndTime;
              

                // Save the changes in the database
                _db.Shifts.Update(shiftFromDb);
                _db.SaveChanges();
                return RedirectToAction("Index"); // Or wherever you want to redirect
            }

            // Populate dropdown for assistant selection again if validation fails
            PopulateAssistantsDropdown(shift);
            return View(shift); // If validation fails, return the view with the model
        }



        [HttpDelete]
        public async Task<IActionResult> DeleteShift(int id)
        {
            var shift = await _db.Shifts.FindAsync(id);
            if (shift == null)
            {
                return NotFound();
            }

            _db.Shifts.Remove(shift);
            await _db.SaveChangesAsync();

            return Ok();
        }


    }
}
