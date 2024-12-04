using HospitalManagement.Data;
using HospitalManagement.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace HospitalManagement.Controllers
{
    public class BookingController : Controller
    {
        private readonly ApplicationDbContext _db;
        private readonly UserManager<IdentityUser> _userManager;

        public BookingController(ApplicationDbContext db, UserManager<IdentityUser> userManager)
        {
            _db = db;
            _userManager = userManager;
        }

        public IActionResult BookAppointment()
        {
            return View();
        }

        // API : Renvoie les créneaux disponibles pour le calendrier
        [HttpGet]

        [HttpGet]
        public JsonResult GetAvailableSlots()
        {
            var availableSlots = _db.Appointments
                .Where(a => a.Status == "Available") // Filtrer par statut "Available"
                .Select(a => new
                {
                    id = a.AppointmentId,
                    title = $"{a.FacultyMember.FirstName} {a.FacultyMember.LastName} " +
                    $" ({a.FacultyMember.DepartmentHead.DepartmentName ?? "Any"})",
                    start = a.AppointmentDate.Date.Add((TimeSpan)a.ShiftStartTime),  // Combine date + time
                    end = a.AppointmentDate.Date.Add((TimeSpan)a.ShiftEndTime)       // Combine date + time
                })
                .ToList();

            return Json(availableSlots); 
        }



        [HttpGet]
        public IActionResult ConfirmBooking(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }

            var appointment = _db.Appointments
        .Include(a => a.FacultyMember) // Include FacultyMember
        .Include(a => a.FacultyMember.Department) // Ensure Department is included
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


        // GET: List all booked appointments
        public IActionResult Index()
        {
            var userId = User.Identity.Name; // or User.FindFirst(ClaimTypes.NameIdentifier).Value

            var bookedAppointments = _db.BookedAppointments
                .Where(b => b.UserId == userId) // Filter by the logged-in user’s UserId
                .Include(b => b.Appointment)
                
                .ToList();

            return View(bookedAppointments); // Display the list of booked appointments
        }


        // GET: Edit a booked appointment
        [HttpGet]
        public IActionResult Edit(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }

            var bookedAppointment = _db.BookedAppointments
               // .Include(b => b.Assistant)
                .Include(b => b.Appointment)
                .FirstOrDefault(b => b.BookedAppointmentId == id);

            if (bookedAppointment == null)
            {
                return NotFound();
            }

            return View(bookedAppointment); // Displays the edit form for the booking
        }

        // POST: Update a booked appointment
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int id, string description)
        {
            var bookedAppointment = _db.BookedAppointments.FirstOrDefault(b => b.BookedAppointmentId == id);

            if (bookedAppointment == null)
            {
                return NotFound();
            }

            bookedAppointment.Description = description;
            bookedAppointment.BookingDate = DateTime.Now;

            _db.SaveChanges();

            return RedirectToAction("Index"); // Redirects to the list of booked appointments
        }

        // GET: Delete a booked appointment
        [HttpGet]
        public IActionResult Delete(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }

            var bookedAppointment = _db.BookedAppointments
             .Include(b => b.Appointment)
             //   .Include(b => b.Assistant)
                .FirstOrDefault(b => b.BookedAppointmentId == id);

            if (bookedAppointment == null)
            {
                return NotFound();
            }

            return View(bookedAppointment); // Displays the delete confirmation
        }

        // POST: Delete a booked appointment
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            var bookedAppointment = _db.BookedAppointments.Find(id);

            if (bookedAppointment == null)
            {
                return NotFound();
            }

            _db.BookedAppointments.Remove(bookedAppointment);
            _db.SaveChanges();

            return RedirectToAction("Index"); // Redirects to the list of booked appointments
        }
    }
}