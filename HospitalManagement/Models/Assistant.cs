namespace HospitalManagement.Models
{
    public class Assistant
    {
        public int AssistantId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string Address { get; set; }

        // Foreign Key to Department
        public int DepartmentId { get; set; }
        public Department Department { get; set; }

        // Shift timing
        public DateTime ShiftStartTime { get; set; }
        public DateTime ShiftEndTime { get; set; }

        // Navigation property for schedules
        public ICollection<Schedule> Schedules { get; set; }

        // Navigation property for appointments
        public ICollection<Appointment> Appointments { get; set; }
    }

}
