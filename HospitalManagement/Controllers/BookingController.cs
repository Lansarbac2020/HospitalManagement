﻿using HospitalManagement.Data;
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
            return View();
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


            // Create a new BookedAppointment
            var bookedAppointment = new BookedAppointment
            {
                AppointmentId = appointment.AppointmentId,
                AssistantId = appointment.AssistantId,
                Description = description,
                BookingDate = DateTime.Now,
                Status = "Booked"
            };
            // Add BookedAppointment to the database
            _db.BookedAppointments.Add(bookedAppointment);
            _db.SaveChanges();

            return RedirectToAction("Index"); // Redirige vers la liste des rendez-vous
        }
        // GET: List all booked appointments
        public IActionResult Index()
        {
            var bookedAppointments = _db.BookedAppointments
                .Include(b => b.Appointment)
                .Include(b => b.Assistant)
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
                .Include(b => b.Assistant)
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
                .Include(b => b.Assistant)
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