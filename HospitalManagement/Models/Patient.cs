using Microsoft.AspNetCore.Identity;
using System;
using System.ComponentModel.DataAnnotations;

namespace HospitalManagement.Models
{
    public class Patient
    {
        // Identifiant unique du patient (clé primaire)
        public int Id { get; set; }

        // Prénom du patient
        [Required]
        [StringLength(100)]
        public string? FirstName { get; set; }

        // Nom de famille du patient
        [Required]
        [StringLength(100)]
        public string LastName { get; set; }

        // Email du patient (pour lier à l'utilisateur)
        [Required]
        [EmailAddress]
        public string Email { get; set; }

        // Adresse du patient
        [StringLength(250)]
        public string? Address { get; set; }

        // Numéro de téléphone du patient
        [Phone]
        public string? PhoneNumber { get; set; }

        // Lier ce patient à un utilisateur d'Identity (via UserId)
        public string? UserId { get; set; }

        // Navigation vers l'utilisateur (relier à IdentityUser)
        public virtual IdentityUser User { get; set; }
    }
}
