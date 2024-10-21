using System;

namespace HospitalManagement.Models
{
    public class Appointment
    {
        public int AppointmentId { get; set; }

        public int AssistantId { get; set; }
        public Assistant Assistant { get; set; }

        public int FacultyMemberId { get; set; }
        public FacultyMember FacultyMember { get; set; }

        public DateTime Date { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
    }
}
