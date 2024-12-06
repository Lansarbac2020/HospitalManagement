namespace HospitalManagement.Models
{
    public class BookedAppointment
    {
        public int BookedAppointmentId { get; set; }  // Identifiant unique pour la réservation

        // Propriétés liées à l'Appointment
        public int AppointmentId { get; set; }
        public string UserId { get; set; }  // Lié à IdentityUser

        public Appointment Appointment { get; set; }  // Navigation vers le modèle Appointment

        // Statut de la réservation
        public string Status { get; set; }

        // Date et heure de la réservation
        public DateTime BookingDate { get; set; }  // Date et heure quand la réservation a été faite

        // Description ou notes supplémentaires pour la réservation
        public string Description { get; set; }  // Description de la réservation, peut-être des détails spécifiques
    }
}
