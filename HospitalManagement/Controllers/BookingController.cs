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
                .Where(a => a.Status == "Available")
                .Select(a => new
                {
                    id = a.AppointmentId,
                    title = $"{a.Doctor.FirstName} {a.Doctor.LastName}",
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

            // Check if the user is authenticated
            var userId = _userManager.GetUserId(User);
            if (userId == null)
            {
                // Redirect to login page if the user is not authenticated
                return RedirectToAction("Login", "Account");
            }

            // Mettre à jour l'état de l'appointment
            appointment.Status = "Booked";
            appointment.UpdatedAt = DateTime.Now;

            // Créer une nouvelle entrée dans BookedAppointments
            var bookedAppointment = new BookedAppointment
            {
                AppointmentId = appointment.AppointmentId,
                UserId = userId, // Use the authenticated user ID
                BookingDate = DateTime.Now,
                Description = description,
                Status = "Confirmed"
            };

          
            _db.BookedAppointments.Add(bookedAppointment);

            _db.SaveChanges();

            return RedirectToAction("Index"); 
        }




        // GET: List all booked appointments
        public IActionResult Index()
        {
            var userId = _userManager.GetUserId(User); 
                                                      

            var bookedAppointments = _db.BookedAppointments
     .Where(b => b.UserId == userId)
     .Include(b => b.Appointment)
         .ThenInclude(a => a.Doctor) // Inclure Doctor
         .ThenInclude(d => d.Department) // Inclure Department via Doctor
     .ToList();

            if (bookedAppointments == null || !bookedAppointments.Any())
            {
                //Console.WriteLine("No appointments found for user: " + userId);
                return RedirectToAction("Login", "Account");
            }

            return View(bookedAppointments);
           
        }





        // GET: Edit a booked appointment (for description only)
        [HttpGet]
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

            return View(bookedAppointment); // Passes the booked appointment to the view
        }


        // POST: Edit a booked appointment (update description only)
        // POST: Edit a booked appointment (update description only)
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int id, string description)
        {
            if (string.IsNullOrEmpty(description))
            {
                ModelState.AddModelError("description", "Description cannot be empty.");
            }

            if (ModelState.IsValid)
            {
                var bookedAppointment = _db.BookedAppointments
                    .FirstOrDefault(b => b.BookedAppointmentId == id);

                if (bookedAppointment == null)
                {
                    return NotFound();
                }

                // Update the description only
                bookedAppointment.Description = description;
              //  bookedAppointment. = DateTime.Now;

                // Save the changes
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

                return RedirectToAction(nameof(Index)); // Redirects to the list of booked appointments
            }

            return View(); // Return to the edit view if the model is invalid
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