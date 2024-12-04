using HospitalManagement.Models;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace HospitalManagement.Models
{
    public class Department
    {
        [Key]
        public int DepartmentId { get; set; }

        [Required(ErrorMessage = "Department name is required.")]
        public string DepartmentName { get; set; }

        [Required]
        public int AvailableBeds { get; set; }

        public int PatientCount { get; set; }

        // Foreign key for FacultyMember (the department head)
        public int? FacultyMemberId { get; set; }

        // Navigation property for FacultyMember (the department head)
        public virtual FacultyMember? FacultyMember { get; set; }

        // Navigation property for assistants
        public ICollection<Assistant> Assistants { get; set; } = new List<Assistant>();
    }


}
