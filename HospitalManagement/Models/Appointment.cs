using HospitalManagement.Models;
using System;
using System.ComponentModel.DataAnnotations;

namespace HospitalManagement.Models
{
    public class Appointment
    {
        [Key]
        public int AppointmentId { get; set; }

        // Foreign key to Assistant
        public int AssistantId { get; set; }
        public virtual Assistant Assistant { get; set; }

        // Foreign key to Faculty Member
        public int FacultyMemberId { get; set; }
        public virtual FacultyMember FacultyMember { get; set; }

        public DateTime AppointmentDate { get; set; }

        // Consider adding validation attributes
        [StringLength(500)] // Limit the description length if needed
        public string Description { get; set; }

        // Additional properties for appointment management
        public string Status { get; set; } // E.g., Pending, Confirmed, Cancelled

        // Optional: Add a property for appointment duration
        public TimeSpan Duration { get; set; } // Duration of the appointment
    }
}
