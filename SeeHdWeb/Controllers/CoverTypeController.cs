using Microsoft.AspNetCore.Mvc;
using SeeHdWeb.Data;
using SeeHdWeb.Models;

namespace SeeHdWeb.Controllers
{
    public class CoverTypeController : Controller
    {
        private readonly ApplicationDbContext _db;
        public CoverTypeController(ApplicationDbContext db)
        {
            _db = db;
        }
        public IActionResult Index()
        {
            IEnumerable<CoverType> objList = _db.CoverTypeCategories.ToList();
            return View(objList);
        }

        //Create Get
        public IActionResult Create()
        {
            return View();
        }
        //Create Post
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(CoverType obj)
        {
            if(ModelState.IsValid)
            {
                _db.CoverTypeCategories.Add(obj);
                _db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(obj);
        }

        //Edit Get
        public IActionResult Edit(int? id)
        {
            if(id == null || id == 0)
            {
                return NotFound();
            }
            var coverTypeFromDb = _db.CoverTypeCategories.Find(id);
            if(coverTypeFromDb == null)
            {
                return NotFound();
            }
            return View(coverTypeFromDb);
        }
        //Edit Post
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(CoverType obj)
        {
            if (ModelState.IsValid)
            {
                _db.CoverTypeCategories.Update(obj);
                _db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(obj);
        }
        //Delete Post
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Delete(int? id)
        {
            var coverTypeFromDb = _db.CoverTypeCategories.Find(id);
            _db.CoverTypeCategories.Remove(coverTypeFromDb);
            _db.SaveChanges();
            return RedirectToAction("Index");
        }
    }
}
