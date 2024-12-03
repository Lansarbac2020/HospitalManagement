using HospitalManagement.Data;
using HospitalManagement.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

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
                .Include(a => a.FacultyMember) // Only include FacultyMember
                .ToList();

            return View(appointments);
        }

        // GET: Create Appointment
        [HttpGet]
        public IActionResult CreateAppointment()
        {
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
        public async Task<IActionResult> CreateAppointment(Appointment appointment)
        {
            if (ModelState.IsValid)
            {
                _db.Add(appointment);
                await _db.SaveChangesAsync();
                return RedirectToAction(nameof(Index)); // Redirect to the appropriate view after saving
            }

            // If model state is invalid, reload the list of faculty members and return to view
            ViewBag.FacultyMembers = new SelectList(
                _db.FacultyMembers.Select(f => new { f.FacultyId, FullName = f.FirstName + " " + f.LastName }),
                "FacultyId",
                "FullName"
            );
            return View(appointment);
        }

        // GET: Edit Appointment
        public IActionResult EditAppointment(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }

            // Retrieve the appointment from the database
            Appointment appointmentFromDb = _db.Appointments
                .Include(a => a.FacultyMember) // Only include FacultyMember
                .FirstOrDefault(a => a.AppointmentId == id);

            if (appointmentFromDb == null)
            {
                return NotFound();
            }

            // Populate the dropdown for faculty members
            ViewBag.FacultyMembers = new SelectList(
                _db.FacultyMembers.Select(f => new { f.FacultyId, FullName = f.FirstName + " " + f.LastName }),
                "FacultyId",
                "FullName",
                appointmentFromDb.FacultyMemberId // Set selected value
            );

            return View(appointmentFromDb);  // Return the view with the current appointment data
        }

        // POST: Edit Appointment
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult EditAppointment(Appointment appointment)
        {
            if (ModelState.IsValid)
            {
                // Ensure the correct FacultyMemberId is provided
                if (appointment.FacultyMemberId == 0)
                {
                    ModelState.AddModelError("", "Faculty Member must be selected.");
                    return View(appointment);  // Return the view with error messages
                }

                // Update the existing appointment in the database
                _db.Appointments.Update(appointment);
                _db.SaveChanges();

                return RedirectToAction("Index");  // Redirect to the list of appointments
            }

            // Re-populate ViewBag in case of validation failure
            ViewBag.FacultyMembers = new SelectList(
                _db.FacultyMembers.Select(f => new { f.FacultyId, FullName = f.FirstName + " " + f.LastName }),
                "FacultyId",
                "FullName",
                appointment.FacultyMemberId
            );

            return View(appointment);  // Return the view if model validation fails
        }

        // GET: Delete Appointment
        public IActionResult DeleteAppointment(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }

            // Retrieve the appointment from the database
            Appointment appointmentFromDb = _db.Appointments
                .Include(a => a.FacultyMember) // Only include FacultyMember
                .FirstOrDefault(a => a.AppointmentId == id);

            if (appointmentFromDb == null)
            {
                return NotFound();
            }

            return View(appointmentFromDb);  // Return the view to confirm deletion
        }

        // POST: Delete Appointment
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            if (id == 0)
            {
                return BadRequest("Invalid ID");
            }

            var appointment = _db.Appointments.Find(id);
            if (appointment == null)
            {
                return NotFound();
            }

            _db.Appointments.Remove(appointment);
            _db.SaveChanges();

            return RedirectToAction("Index");
        }

        // API: Renvoie les créneaux disponibles pour le calendrier
        [HttpGet]
        public JsonResult GetAvailableSlots()
        {
            var availableSlots = _db.Appointments
                .Where(a => a.Status == "Available") // Filter by Available status
                .Select(a => new
                {
                    id = a.AppointmentId,
                    title = $"{a.FacultyMember.FirstName} {a.FacultyMember.LastName}",
                    start = a.AppointmentDate.Date + a.ShiftStartTime,
                    end = a.AppointmentDate.Date + a.ShiftEndTime
                })
                .ToList();

            return Json(availableSlots); // Return data for the calendar
        }

        // GET: Confirmer la réservation
        [HttpGet]
        public IActionResult ConfirmBooking(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }

            var appointment = _db.Appointments
                .Include(a => a.FacultyMember) // Only include FacultyMember
                .FirstOrDefault(a => a.AppointmentId == id && a.Status == "Available");

            if (appointment == null)
            {
                return NotFound();
            }

            return View(appointment); // Show the confirmation form for booking
        }

        // POST: Confirmer la réservation
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult ConfirmBooking(int id, string description)
        {
            var appointment = _db.Appointments.FirstOrDefault(a => a.AppointmentId == id && a.Status == "Available");

            if (appointment == null)
            {
                return NotFound();
            }

            // Mark the appointment as booked
            appointment.Status = "Booked";
            appointment.Description = description;
            appointment.UpdatedAt = DateTime.Now;

            _db.SaveChanges();

            return RedirectToAction("Index"); // Redirect to the list of appointments
        }
    }
}
