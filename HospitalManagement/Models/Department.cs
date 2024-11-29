using HospitalManagement.Models;
using System.ComponentModel;

namespace HospitalManagement.Models
{
    public class Department
    {
        public int DepartmentId { get; set; }

        [DisplayName("Departman Name")]
        public string DepartmentName { get; set; }

        [DisplayName("Patient Count")]
        public int PatientCount { get; set; }

        [DisplayName("Available Beds")]
        public int AvailableBeds { get; set; }

        // Navigation property for the department's head (one-to-one relationship)
        public int? FacultyMemberId { get; set; } // Nullable for departments without a head initially
        public FacultyMember? FacultyMember { get; set; }

        // Navigation property for assistants
        public ICollection<Assistant> Assistants { get; set; } = new List<Assistant>();
    }


}
