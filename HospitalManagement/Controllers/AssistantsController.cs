using HospitalManagement.Data;
using HospitalManagement.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;

namespace HospitalManagement.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AssistantsController : Controller
    {
        private readonly ApplicationDbContext _db;

        public AssistantsController(ApplicationDbContext db)
        {
            _db = db;
        }

        // GET: Assistants
        public IActionResult Index()
        {
            List<Assistant> objAssistantList = _db.Assistants
                .Include(a => a.Department)
                .ToList();

            return View(objAssistantList);
        }

        // GET: Create Assistant
        public IActionResult CreateAssistant()
        {
            ViewBag.Departments = _db.Departments.ToList();
            return View(new Assistant());
        }

        // POST: Create Assistant
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult CreateAssistant(Assistant assistant)
        {
            if (ModelState.IsValid)
            {
                _db.Assistants.Add(assistant);
                _db.SaveChanges();

                return RedirectToAction("Index");
            }

            ViewBag.Departments = _db.Departments.ToList();
            return View(assistant);
        }

        // GET: Edit Assistant
        public IActionResult EditAssistant(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }

            Assistant assistantFromDb = _db.Assistants.Find(id);
            if (assistantFromDb == null)
            {
                return NotFound();
            }

            ViewBag.Departments = _db.Departments.ToList();
            return View(assistantFromDb);
        }

        // POST: Edit Assistant
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult EditAssistant(Assistant assistant)
        {
            if (ModelState.IsValid)
            {
                _db.Assistants.Update(assistant); // Correctly updating the assistant instead of adding
                _db.SaveChanges();

                return RedirectToAction("Index");
            }

            ViewBag.Departments = _db.Departments.ToList();
            return View(assistant);
        }

        // GET: Delete Assistant
        public IActionResult DeleteAssistant(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }

            Assistant assistantFromDb = _db.Assistants.Include(a => a.Department).FirstOrDefault(a => a.AssistantId == id);
            if (assistantFromDb == null)
            {
                return NotFound();
            }

            return View(assistantFromDb); // Return the assistant to the view to show details in modal
        }

        // POST: Delete Assistant
        [HttpPost, ActionName("DeleteConfirmed")]
        [ValidateAntiForgeryToken]
        // POST: Delete Assistant
        public IActionResult DeleteConfirmed(int id)
        {
            var assistantFromDb = _db.Assistants.Find(id);
            if (assistantFromDb == null)
            {
                return NotFound();
            }

            // Check if there are any related Shifts
            var relatedShifts = _db.Shifts.Where(s => s.AssistantId == id).ToList();

            if (relatedShifts.Any())
            {
                // Handle the related shifts - for example, you can delete them or set the AssistantId to null.
                _db.Shifts.RemoveRange(relatedShifts);

               
            }

            
            _db.Assistants.Remove(assistantFromDb);
            _db.SaveChanges();

            return RedirectToAction("Index"); // Redirect back to the list of assistants after deletion
        }

    }
}
