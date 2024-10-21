﻿using System.ComponentModel.DataAnnotations;

namespace HospitalManagement.Models
{
    public class FacultyMember
    {
        [Key]
        public int FacultyId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }

        // Foreign Key to Department
        public int DepartmentId { get; set; }
        public Department Department { get; set; }

        // Navigation property for appointments
        public ICollection<Appointment> Appointments { get; set; }
    }

}
