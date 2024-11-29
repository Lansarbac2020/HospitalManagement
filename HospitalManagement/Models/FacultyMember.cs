using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace HospitalManagement.Models
{
    public class FacultyMember
    {
        [Key]
        public int FacultyId { get; set; }

        [Required(ErrorMessage = "First name is required.")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "Last name is required.")]
        public string LastName { get; set; }

        [Required(ErrorMessage = "Email is required.")]
        [EmailAddress(ErrorMessage = "Invalid email format.")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Phone number is required.")]
        [Phone(ErrorMessage = "Invalid phone number format.")]
        public string Phone { get; set; }

        // Foreign Key for the Department they belong to
        public int DepartmentId { get; set; }
        public virtual Department? Department { get; set; }

        // Navigation property for the department they lead
        public virtual Department? HeadOfDepartment { get; set; } // One-to-one relationship
        [Key]

        // Navigation property for appointments
        public virtual ICollection<Appointment> Appointments { get; set; } = new List<Appointment>(); // Initialize the collection
    }
}
