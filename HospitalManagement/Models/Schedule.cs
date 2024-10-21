namespace HospitalManagement.Models
{
    public class Schedule
    {
        public int ScheduleId { get; set; }

        // Foreign Key to Assistant
        public int AssistantId { get; set; }
        public Assistant Assistant { get; set; }

        // Foreign Key to Department
        public int DepartmentId { get; set; }
        public Department Department { get; set; }

        public DateTime Date { get; set; }
        public DateTime ShiftStartTime { get; set; }
        public DateTime ShiftEndTime { get; set; }
    }

}
