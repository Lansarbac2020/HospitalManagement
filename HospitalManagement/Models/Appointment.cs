using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HospitalManagement.Models
{
    public class Appointment
    {
        [Key]
        public int AppointmentId { get; set; }

        // Foreign key to Faculty Member (Department Head)
        [Required]
        public int FacultyMemberId { get; set; }
        public virtual FacultyMember? FacultyMember { get; set; }

        // Appointment details
        [Required]
        [DataType(DataType.Date)]
        public DateTime AppointmentDate { get; set; }

        [StringLength(500)]
        public string? Description { get; set; }

        // Department the appointment belongs to
        public int? DepartmentId { get; set; } // Foreign Key to Department
        public Department? Department { get; set; } // Navigation Property to Department

        // Shift start and end times
        [DataType(DataType.Time)]
        public TimeSpan? ShiftStartTime { get; set; }
        public TimeSpan? ShiftEndTime { get; set; }

        // Appointment status (e.g., Available, Completed, etc.)
        public string Status { get; set; } = "Available";

        // Optional duration
        [DataType(DataType.Time)]
        public DateTime Days { get; set; }

        // Audit fields
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public DateTime UpdatedAt { get; set; }
    }

}
