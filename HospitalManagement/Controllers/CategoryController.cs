using HospitalManagement.Data;
using HospitalManagement.Models;
using Microsoft.AspNetCore.Mvc;

namespace HospitalManagement.Controllers
{
    public class CategoryController : Controller
    {
     private readonly ApplicationDbContext _db;
        public CategoryController(ApplicationDbContext db)
        {
            _db = db;
        }
        public IActionResult Index()
        {
            List<Category> objCategoryList=_db.Categories.ToList();

            return View(objCategoryList); 
        }
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Create(Category obj)
        {
            if (obj.Name == obj.Displayorder)
            {
                ModelState.AddModelError("name", "Name and display order cannot be same");
            }
            if (ModelState.IsValid)
            {
                _db.Categories.Add(obj);
                _db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View();
        }

    }
}
