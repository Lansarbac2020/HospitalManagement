using HospitalManagement.Models;
using System.ComponentModel;

public class Department
{
    public int DepartmentId { get; set; }

    [DisplayName("Departman Name")]
    public string DepartmentName { get; set; }

    [DisplayName("Patient Count")]
    public int PatientCount { get; set; }

    [DisplayName("Available Beds")]
    public int AvailableBeds { get; set; } // Fixed label from "Patien Name"

    // Navigation property for the related Schedules
    public ICollection<Schedule> Schedules { get; set; } = new List<Schedule>(); // Initialize to avoid null reference
}
