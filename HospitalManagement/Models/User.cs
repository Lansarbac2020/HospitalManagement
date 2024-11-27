using System.ComponentModel.DataAnnotations;

namespace HospitalManagement.Models
{
    public class User
    {
        public int Id { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        public string Role { get; set; } = "Public"; // Default role is "Public"
    }
}
