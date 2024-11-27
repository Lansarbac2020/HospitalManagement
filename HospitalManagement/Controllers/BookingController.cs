using HospitalManagement.Data;
using HospitalManagement.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;

namespace HospitalManagement.Controllers
{
    public class BookingController : Controller
    {
        private readonly ApplicationDbContext _db;

        public BookingController(ApplicationDbContext db)
        {
            _db = db;
        }
        public IActionResult BookAppointment()
        {
            return View(); // Cette action retourne la vue `BookAppointment.cshtml`
        }
        // API : Renvoie les créneaux disponibles pour le calendrier
        [HttpGet]
        public JsonResult GetAvailableSlots()
        {
            var availableSlots = _db.Appointments
                .Where(a => a.Status == "Pending") // Seules les disponibilités
                .Select(a => new
                {
                    id = a.AppointmentId,
                    title = $"{a.Assistant.FirstName} {a.Assistant.LastName}",
                    start = a.AppointmentDate.Date + a.ShiftStartTime,
                    end = a.AppointmentDate.Date + a.ShiftEndTime
                })
                .ToList();

            return Json(availableSlots); // Retourne les données pour le calendrier
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
                .Include(a => a.Assistant)
                .Include(a => a.FacultyMember)
                .FirstOrDefault(a => a.AppointmentId == id && a.Status == "Pending");

            if (appointment == null)
            {
                return NotFound();
            }

            return View(appointment); // Affiche un formulaire pour confirmer la réservation
        }

        // POST: Confirmer la réservation
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult ConfirmBooking(int id, string description)
        {
            var appointment = _db.Appointments.FirstOrDefault(a => a.AppointmentId == id && a.Status == "Pending");

            if (appointment == null)
            {
                return NotFound();
            }

            // Marque le rendez-vous comme réservé
            appointment.Status = "Booked";
            appointment.Description = description;
            appointment.UpdatedAt = DateTime.Now;

            _db.SaveChanges();

            return RedirectToAction("Index"); // Redirige vers la liste des rendez-vous
        }


    }
}