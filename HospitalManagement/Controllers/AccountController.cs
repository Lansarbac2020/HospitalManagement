using HospitalManagement.Data;
using HospitalManagement.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Threading.Tasks;

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

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                // Vérifiez si l'email appartient à un assistant
                var isAssistantEmail = _context.Assistants.Any(a => a.Email == model.Email);
                if (!isAssistantEmail)
                {
                    ModelState.AddModelError("Email", "Kaydolmak için bir Assistant e-postası kullanmalısınız.");
                    return View(model);
                }

                // Vérifiez si le rôle "Patient" existe, sinon créez-le
                if (!await _roleManager.RoleExistsAsync("Patient"))
                {
                    var role = new IdentityRole("Patient");
                    await _roleManager.CreateAsync(role);
                }

                // Créez l'utilisateur avec l'email et le mot de passe
                var user = new IdentityUser { UserName = model.Email, Email = model.Email };
                var result = await _userManager.CreateAsync(user, model.Password);

                if (result.Succeeded)
                {
                    // Assignez le rôle "Patient" à l'utilisateur
                    await _userManager.AddToRoleAsync(user, "Patient");

                    // Liez le compte utilisateur à un patient existant ou créez-en un
                    var patient = new Patient
                    {
                        FirstName = model.FirstName,
                        LastName = model.LastName,
                        Email = model.Email,
                        UserId = user.Id // Lier le UserId à l'utilisateur
                    };

                    _context.Patients.Add(patient);
                    await _context.SaveChangesAsync();

                    // Connectez l'utilisateur après l'inscription
                    await _signInManager.SignInAsync(user, isPersistent: false);

                    return RedirectToAction("Index", "Home");
                }

                // Ajouter les erreurs à ModelState si la création échoue
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }

            return View(model);
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
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

                // Messages d'échec spécifiques
                if (result.IsNotAllowed)
                {
                    ModelState.AddModelError("", "E-postanız onaylanmadı.");
                }
                else if (result.IsLockedOut)
                {
                    ModelState.AddModelError("", "Hesabınız kilitlendi.");
                }
                else
                {
                    ModelState.AddModelError("", "Geçersiz bağlantı denemesi.");
                }
            }

            return View(model);
        }

        
        [HttpGet]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Login");
        }

    }
}
