using HospitalManagement.Data;
using HospitalManagement.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore; // Add this for Include method
using System.Collections.Generic; // Add this for List<T>
using System.Linq; // Add this for ToList()

namespace HospitalManagement.Controllers
{
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
                .Include(a => a.Department) // Eagerly load the Department
                .ToList();

            return View(objAssistantList);
        }


        // GET: Create Assistant
        public IActionResult CreateAssistant()
        {
            // Fetch departments from the database and pass them to the view
            ViewBag.Departments = _db.Departments.ToList(); // Ensure this is populated
            return View(new Assistant()); // Return an empty Assistant object to the view
        }

        // POST: Create Assistant
        [HttpPost]
        [ValidateAntiForgeryToken] // This helps prevent CSRF attacks
        public IActionResult CreateAssistant(Assistant assistant)
        {
            if (ModelState.IsValid)
            {
                // Save the assistant data
                _db.Assistants.Add(assistant);
                _db.SaveChanges();

                // After saving, reload the assistant with its related department
                _db.Entry(assistant).Reference(a => a.Department).Load(); // This loads the Department navigation property

                return RedirectToAction("Index");
            }

            // Repopulate departments if model state is invalid
            ViewBag.Departments = _db.Departments.ToList();
            return View(assistant);
        }



        // Additional actions (e.g., Edit, Delete, Details) can go here...
    }
}
