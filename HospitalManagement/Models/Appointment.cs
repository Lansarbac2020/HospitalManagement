using HospitalManagement.Models.HospitalManagement.Models;
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
        public int FacultyMemberId { get; set; }  // Add this line
        public virtual FacultyMember FacultyMember { get; set; }  // Add this line

        public DateTime AppointmentDate { get; set; }
        public string Description { get; set; }
    }
}
