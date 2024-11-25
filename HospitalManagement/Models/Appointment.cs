using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HospitalManagement.Models
{
    public class Appointment
    {
        [Key]
        public int AppointmentId { get; set; }

        // Foreign key to Assistant
        [Required]
        public int AssistantId { get; set; }
        public virtual Assistant? Assistant { get; set; }

        // Foreign key to Faculty Member
        [Required]
        public int FacultyMemberId { get; set; }
        public virtual FacultyMember? FacultyMember { get; set; }

        // Appointment details
        [Required]
        [DataType(DataType.Date)]
        public DateTime AppointmentDate { get; set; }

        [StringLength(500)]
        public  string? Description { get; set; }

        // Appointment status
        public string Status { get; set; } = "Pending";

        // Optional duration
        [DataType(DataType.Time)]
        public TimeSpan Duration { get; set; }

        // Audit fields
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public DateTime UpdatedAt { get; set; }
    }
}
