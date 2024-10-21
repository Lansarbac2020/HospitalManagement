using System.ComponentModel.DataAnnotations;

namespace HospitalManagement.Models
{
    public class Category
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        public string Displayorder { get; set; }
    }
}
