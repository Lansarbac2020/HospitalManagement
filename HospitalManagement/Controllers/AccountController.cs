using HospitalManagement.Data;
using HospitalManagement.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace HospitalManagement.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly ApplicationDbContext _context;

        public AccountController(UserManager<IdentityUser> userManager,
                                 SignInManager<IdentityUser> signInManager,
                                 RoleManager<IdentityRole> roleManager,
                                 ApplicationDbContext context)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
            _context = context;
        }

        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        // Méthode pour l'enregistrement d'un utilisateur
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                // Vérifiez si le rôle "Patient" existe, sinon le créer
                if (!await _roleManager.RoleExistsAsync("Patient"))
                {
                    var role = new IdentityRole("Patient");
                    await _roleManager.CreateAsync(role);
                }

                // Création de l'utilisateur via Identity
                var user = new IdentityUser { UserName = model.Email, Email = model.Email };
                var result = await _userManager.CreateAsync(user, model.Password);

                if (result.Succeeded)
                {
                    // Assigner un rôle "Patient"
                    await _userManager.AddToRoleAsync(user, "Patient");

                    // Création du patient et liaison avec l'utilisateur
                    var patient = new Patient
                    {
                        FirstName = model.FirstName,
                        LastName = model.LastName,
                        Email = model.Email,
                        UserId = user.Id // Lier l'utilisateur avec le patient
                    };

                    _context.Patients.Add(patient);
                    await _context.SaveChangesAsync();

                    // Connexion de l'utilisateur après inscription
                    await _signInManager.SignInAsync(user, isPersistent: false);

                    return RedirectToAction("Index", "Home"); // Page d'accueil après enregistrement
                }

                // Ajouter les erreurs à ModelState si l'inscription échoue
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }

            // Si l'état du modèle n'est pas valide, retourner à la vue d'enregistrement
            return View(model);
        }



        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                // Tentative de connexion
                var result = await _signInManager.PasswordSignInAsync(model.Email, model.Password, false, false);

                if (result.Succeeded)
                {
                    return RedirectToAction("Index", "Home");
                }

                // Messages pour les différents cas d'échec
                if (result.IsNotAllowed)
                {
                    ModelState.AddModelError("", "Votre email n'est pas confirmé.");
                }
                else if (result.IsLockedOut)
                {
                    ModelState.AddModelError("", "Votre compte est verrouillé.");
                }
                else
                {
                    ModelState.AddModelError("", "Tentative de connexion invalide.");
                }
            }

            return View(model);
        }

        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Login");
        }
    }
}
