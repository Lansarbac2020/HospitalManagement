//using Microsoft.AspNetCore.Authorization;
//using Microsoft.AspNetCore.Identity;
//using Microsoft.AspNetCore.Mvc;
//using System.Collections.Generic;
//using System.Threading.Tasks;

//namespace HospitalManagement.Controllers
//{
//    [Authorize(Roles = "Admin")]  // Restrict access to Admin role only
//    public class AdminController : Controller
//    {
//        private readonly UserManager<IdentityUser> _userManager;

//        public AdminController(UserManager<IdentityUser> userManager)
//        {
//            _userManager = userManager;
//        }

//        // Admin Dashboard
//        public IActionResult Index()
//        {
//            return View();
//        }

//        // Manage Users
//        public async Task<IActionResult> ManageUsers()
//        {
//            var users = _userManager.Users;
//            var userList = new List<IdentityUser>();

//            foreach (var user in users)
//            {
//                var roles = await _userManager.GetRolesAsync(user);
//                userList.Add(new IdentityUser { UserName = user.UserName, Email = user.Email, Roles = roles });
//            }

//            return View(userList);
//        }

//        // Assign Roles to User
//        public async Task<IActionResult> AssignRole(string userId, string role)
//        {
//            var user = await _userManager.FindByIdAsync(userId);
//            if (user != null)
//            {
//                await _userManager.AddToRoleAsync(user, role);
//            }

//            return RedirectToAction("ManageUsers");
//        }

//        // Remove Role from User
//        public async Task<IActionResult> RemoveRole(string userId, string role)
//        {
//            var user = await _userManager.FindByIdAsync(userId);
//            if (user != null)
//            {
//                await _userManager.RemoveFromRoleAsync(user, role);
//            }

//            return RedirectToAction("ManageUsers");
//        }
//    }
//}
