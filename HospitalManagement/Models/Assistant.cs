 using System.Collections.Generic;
 using System.ComponentModel.DataAnnotations;


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

           
            public int DepartmentId { get; set; }

            public DateTime ShiftStartTime { get; set; }
            public DateTime ShiftEndTime { get; set; }

            // Navigation properties
            public virtual Department Department { get; set; }

            // Collection for related appointments
            public virtual ICollection<Appointment> Appointments { get; set; } = new List<Appointment>();
           // Collection for related schedules
        public virtual ICollection<Schedule> Schedules { get; set; } = new List<Schedule>();
    }
    }