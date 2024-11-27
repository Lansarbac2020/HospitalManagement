using System;

namespace HospitalManagement.Models
{
    public class BookedAppointment
    {
        public int BookedAppointmentId { get; set; }  // Unique identifier for the booking

        // Foreign key for the associated appointment
        public int AppointmentId { get; set; }
        public Appointment Appointment { get; set; }  // Navigation property to Appointment

        // Foreign key for the assistant who the appointment is booked with
        public int AssistantId { get; set; }
        public Assistant Assistant { get; set; }  // Navigation property to Assistant (if applicable)

        // Description or any other notes entered by the patient for the appointment
        public string Description { get; set; }

        // Date and time when the appointment was booked
        public DateTime BookingDate { get; set; }

        // Status of the appointment (e.g., confirmed, pending, canceled, etc.)
        public string Status { get; set; }

        // Optional: Add any additional fields you may need, such as payment status, reminder sent, etc.
    }
}

