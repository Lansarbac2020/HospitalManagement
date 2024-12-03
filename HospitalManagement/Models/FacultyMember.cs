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

        // Navigation property for the department they head
        public virtual Department? DepartmentHead { get; set; } // Existing property: Faculty member heads a department.
        public virtual ICollection<Appointment> Appointments { get; set; } = new List<Appointment>();

        // Optional: Add a relationship with the department where appointments are scheduled
        public virtual Department? Department { get; set; } // Add this to reflect the department the faculty member belongs to.

        // Navigation property for the department they head
    }
}
