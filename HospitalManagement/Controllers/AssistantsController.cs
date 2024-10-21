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

        public IActionResult Index()
        {
            // Include the Department data when retrieving the list of assistants
            List<Assistant> objAssistantList = _db.Assistants
                .Include(a => a.Department) // Include related Department data
                .ToList();

            return View(objAssistantList);
        }

        // Other actions (Create, Edit, Delete) can go here...
    }
}
