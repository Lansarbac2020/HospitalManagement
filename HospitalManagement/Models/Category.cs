using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace HospitalManagement.Models
{
    public class Category
    {
        [Key]
        public int Id { get; set; }
        [Required]
        [MaxLength(30, ErrorMessage = "Display order must be between 1-100 bruhh")]
        [DisplayName("Category Name")]
        public string Name { get; set; }
        [DisplayName("Display Order")]
        [Range(1,100, ErrorMessage ="Display order must be between 1-100 bruhh")]
        public string Displayorder { get; set; }
    }
}
