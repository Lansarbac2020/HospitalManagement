using HospitalManagement.Models;

public class Department
{
    public int DepartmentId { get; set; }
    public string DepartmentName { get; set; }
    public int PatientCount { get; set; }
    public int AvailableBeds { get; set; }

    // Navigation property for the related Schedules
    public ICollection<Schedule> Schedules { get; set; } // Add this line
}
