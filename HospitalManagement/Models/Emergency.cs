using System;
using System.ComponentModel.DataAnnotations;

namespace HospitalManagement.Models
{
    
    public class Emergency
    {
        [Key]
        public int EmergencyId { get; set; } // Primary Key

        [Required]
        [StringLength(100)]
        public string Title { get; set; } // Title of the Emergency

        [Required]
        [StringLength(500)]
        public string Description { get; set; } // Detailed Description of the Emergency

        [Required]
        public DateTime DateCreated { get; set; } = DateTime.Now; // Date and Time the Emergency was Created

        
    }

}
