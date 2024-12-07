using HospitalManagement.Data;
using HospitalManagement.Models;
using HospitalManagement.Services;
using Microsoft.AspNetCore.Mvc;

namespace HospitalManagement.Controllers
{
    public class EmergencyController : Controller
    {
        private readonly ApplicationDbContext _db;
        private readonly MailjetService _mailjetService;

        public EmergencyController(ApplicationDbContext db, MailjetService mailjetService)
        {
            _db = db;
            _mailjetService = mailjetService;
        }

        public IActionResult Index()
        {
            
            var emergencies = _db.Emergencies.ToList();

            // Vérifier si des urgences existent
            if (emergencies.Any())
            {
                return View(emergencies);
            }

            
            return View(new List<Emergency>());
        }

        public IActionResult Create()
        {
           
            if (!User.IsInRole("Admin"))
            {
                // Si l'utilisateur n'est pas Admin, rediriger vers une page d'erreur ou d'accès refusé
                return RedirectToAction("Login", "Account");
            }

            return View(); // Afficher le formulaire de création
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Emergency emergency)
        {
           
            if (!User.IsInRole("Admin"))
            {
                
                return RedirectToAction("Login", "Account");
            }

            if (ModelState.IsValid)
            {
                // Sauvegarder l'urgence dans la base de données
                _db.Emergencies.Add(emergency);
                await _db.SaveChangesAsync();

                // Récupérer tous les emails des assistants et des médecins
                var assistantEmails = _db.Assistants.Select(a => a.Email).ToList();
                var doctorEmails = _db.Doctors.Select(d => d.Email).ToList();

                // Fusionner les deux listes et enlever les doublons
                var allEmails = assistantEmails.Concat(doctorEmails).Distinct();

                // Envoyer un email à chaque utilisateur
                foreach (var recipient in allEmails)
                {
                    await _mailjetService.SendEmailAsync(
                        recipient,
                        "Emergency Alert",
                        $"<p>An emergency has been posted:</p>" +
                        $"<strong>{emergency.Title}</strong><br/>{emergency.Description}<br/>" +
                        $"<p>Respond immediately.</p>"
                    );
                }

                return RedirectToAction("Index"); // Rediriger vers la liste des urgences
            }

            // Si la validation du modèle échoue, retourner le même formulaire avec les erreurs
            return View(emergency);
        }
    }
}
