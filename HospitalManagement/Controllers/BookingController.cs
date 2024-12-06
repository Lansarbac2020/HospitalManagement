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
                .Where(a => a.Status == "Available") // Filter by Available status
                .Select(a => new
                {
                    id = a.AppointmentId,
                    title = $"{a.Doctor.FirstName} {a.Doctor.LastName}", // Change to Doctor
                    start = a.AppointmentDate.Date + a.ShiftStartTime,
                    end = a.AppointmentDate.Date + a.ShiftEndTime
                })
                .ToList();

            return Json(availableSlots); // Return data for the calendar
        }



        [HttpGet]
        public IActionResult ConfirmBooking(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }

            var appointment = _db.Appointments
                .Include(a => a.Doctor) // Include Doctor instead of FacultyMember
                 .Include(a => a.Doctor.Department)  // Include Department data

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
            // Récupérer l'appointment disponible
            var appointment = _db.Appointments
                .Include(a => a.Doctor)
                .FirstOrDefault(a => a.AppointmentId == id && a.Status == "Available");

            if (appointment == null)
            {
                return NotFound();
            }

            // Mettre à jour l'état de l'appointment
            appointment.Status = "Booked";
            appointment.UpdatedAt = DateTime.Now;

            // Créer une nouvelle entrée dans BookedAppointments
            var bookedAppointment = new BookedAppointment
            {
                AppointmentId = appointment.AppointmentId,
                UserId = _userManager.GetUserId(User), // Récupérer l'ID de l'utilisateur connecté
                BookingDate = DateTime.Now,
                Description = description,
                Status = "Confirmed" // Statut de la réservation
            };

            // Ajouter dans la table BookedAppointments
            _db.BookedAppointments.Add(bookedAppointment);

            // Sauvegarder les changements
            _db.SaveChanges();

            return RedirectToAction("Index"); // Rediriger vers la liste des rendez-vous réservés
        }



        // GET: List all booked appointments
        public IActionResult Index()
        {
            var userId = _userManager.GetUserId(User); // Utilisez _userManager pour obtenir l'ID de l'utilisateur
                                                       // Récupérer l'ID de l'utilisateur connecté

            var bookedAppointments = _db.BookedAppointments
     .Where(b => b.UserId == userId)
     .Include(b => b.Appointment)
         .ThenInclude(a => a.Doctor) // Inclure Doctor
         .ThenInclude(d => d.Department) // Inclure Department via Doctor
     .ToList();

            if (bookedAppointments == null || !bookedAppointments.Any())
            {
                Console.WriteLine("No appointments found for user: " + userId);
            }

            return View(bookedAppointments);
            // Passer les rendez-vous réservés à la vue
        }





        // GET: Edit a booked appointment (for description only)
        [HttpGet]
        public IActionResult Edit(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }

            var bookedAppointment = _db.BookedAppointments
                .FirstOrDefault(b => b.BookedAppointmentId == id);

            if (bookedAppointment == null)
            {
                return NotFound();
            }

            return View(bookedAppointment); // Affiche le formulaire d'édition
        }

        // POST: Edit a booked appointment (update description only)
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(BookedAppointment bookedAppointment)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    _db.Update(bookedAppointment);
                    _db.SaveChanges();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!_db.BookedAppointments.Any(e => e.BookedAppointmentId == bookedAppointment.BookedAppointmentId))
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

            return View(bookedAppointment); // Retourne à la vue d'édition si le modèle est invalide
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
        [HttpPost, ActionName("DeleteConfirmed")]
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