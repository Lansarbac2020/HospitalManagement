using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HospitalManagement.Models
{
    public class Shift
    {
        [Key]
        public int ShiftId { get; set; }

        [Required]
        public int AssistantId { get; set; }

        [ForeignKey("AssistantId")]
        public virtual Assistant? Assistant { get; set; }

        [Required]
        [DataType(DataType.Date)]
        public DateTime ShiftDate { get; set; }

        [Required]
        [DataType(DataType.Time)]
        public TimeSpan StartTime { get; set; }

        [Required]
        [DataType(DataType.Time)]
        public TimeSpan EndTime { get; set; }
    }
}
