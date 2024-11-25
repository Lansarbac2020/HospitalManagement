using HospitalManagement.Data;
using HospitalManagement.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;

namespace HospitalManagement.Controllers
{
    public class AppointmentsController : Controller
    {
        private readonly ApplicationDbContext _db;

        public AppointmentsController(ApplicationDbContext db)
        {
            _db = db;
        }

        // GET: Appointments
        public IActionResult Index()
        {
            var appointments = _db.Appointments
                .Include(a => a.Assistant)
                .Include(a => a.FacultyMember)
                .ToList();

            return View(appointments);
        }


        // GET: Create Appointment
        [HttpGet]
        public IActionResult CreateAppointment()
        {
            // Fetch assistants and populate the dropdown
            ViewBag.Assistants = new SelectList(
                _db.Assistants.Select(a => new { a.AssistantId, FullName = a.FirstName + " " + a.LastName }),
                "AssistantId",
                "FullName"
            );

            // Fetch faculty members and populate the dropdown
            ViewBag.FacultyMembers = new SelectList(
                _db.FacultyMembers.Select(f => new { f.FacultyId, FullName = f.FirstName + " " + f.LastName }),
                "FacultyId",
                "FullName"
            );

            return View();
        }




        // POST: Create Appointment
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult CreateAppointment(Appointment appointment)
        {
            if (ModelState.IsValid)
            {
                // Ensure that AssistantId and FacultyMemberId are set
                if (appointment.AssistantId == 0 || appointment.FacultyMemberId == 0)
                {
                    // Handle the case where the assistant or faculty member isn't selected
                    ModelState.AddModelError("", "Assistant and Faculty Member must be selected.");
                    return View(appointment);
                }

                // Set CreatedAt and UpdatedAt fields
                appointment.CreatedAt = DateTime.Now;
                appointment.UpdatedAt = DateTime.Now;

                _db.Appointments.Add(appointment);
                _db.SaveChanges();

                return RedirectToAction("Index"); // Redirect to the appointment list or any other page
            }

            // Re-populate ViewBag in case of validation failure
            ViewBag.Assistants = new SelectList(_db.Assistants, "AssistantId", "FirstName");
            ViewBag.FacultyMembers = new SelectList(_db.FacultyMembers, "FacultyMemberId", "FullName");

            return View(appointment);
        }


        // GET: Edit Appointment
        public IActionResult EditAppointment(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }

            Appointment appointmentFromDb = _db.Appointments.Find(id);
            if (appointmentFromDb == null)
            {
                return NotFound();
            }

            ViewBag.Assistants = _db.Assistants.ToList();
            ViewBag.FacultyMembers = _db.FacultyMembers.ToList();
            return View(appointmentFromDb);
        }

        // POST: Edit Appointment
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult EditAppointment(Appointment appointment)
        {
            if (ModelState.IsValid)
            {
                _db.Appointments.Update(appointment); // Correctly updating the appointment instead of adding
                _db.SaveChanges();

                return RedirectToAction("Index");
            }

            ViewBag.Assistants = _db.Assistants.ToList();
            ViewBag.FacultyMembers = _db.FacultyMembers.ToList();
            return View(appointment);
        }

        // GET: Delete Appointment
        public IActionResult DeleteAppointment(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }

            Appointment appointmentFromDb = _db.Appointments
                .Include(a => a.FacultyMember)
                .FirstOrDefault(a => a.AppointmentId == id);

            if (appointmentFromDb == null)
            {
                return NotFound();
            }

            return View(appointmentFromDb);
        }

        // POST: Delete Appointment
        [HttpPost, ActionName("DeleteAppointment")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            Appointment appointmentFromDb = _db.Appointments.Find(id);
            if (appointmentFromDb == null)
            {
                return NotFound();
            }

            _db.Appointments.Remove(appointmentFromDb);
            _db.SaveChanges();

            return RedirectToAction("Index");
        }
    }
}
