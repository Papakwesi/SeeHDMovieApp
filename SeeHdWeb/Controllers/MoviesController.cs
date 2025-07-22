using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using SeeHdWeb.Data;
using SeeHdWeb.Models;
using SeeHdWeb.Models.ViewModels;

namespace SeeHdWeb.Controllers
{
    public class MoviesController : Controller
    {
        private readonly ApplicationDbContext _db;
        private readonly IWebHostEnvironment _hostEnvironment;
        

        public MoviesController(ApplicationDbContext db, IWebHostEnvironment hostEnvironment)
        {
            _db = db;
            _hostEnvironment = hostEnvironment;
            
        }
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Upsert(int? id)
        {
            MovieVM movieVM = new()
            {
                Movie = new Movies(),
                CategoryList = _db.Categories.Select(u => new SelectListItem
                {
                    Text = u.Name,
                    Value = u.Id.ToString()
                }),
                CoverTypeList = _db.CoverTypeCategories.Select(u => new SelectListItem
                {
                    Text = u.Name,
                    Value = u.Id.ToString()
                })
            };

            if (id != null && id != 0)
            {
                movieVM.Movie = _db.Movies.AsNoTracking().FirstOrDefault(u => u.Id == id);
                if (movieVM.Movie == null)
                    return NotFound();
            }

            return View(movieVM);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Upsert(MovieVM movieVM, IFormFile? file)
        {
            if (ModelState.IsValid)
            {
                string wwwRootPath = _hostEnvironment.WebRootPath;
                if (file != null)
                {
                    string fileName = Guid.NewGuid().ToString();
                    var uploads = Path.Combine(wwwRootPath, @"images\movies");
                    var extension = Path.GetExtension(file.FileName);

                    if(movieVM.Movie.ImageUrl != null)
                    {
                        var oldImagePath = Path.Combine(wwwRootPath, movieVM.Movie.ImageUrl.TrimStart('\\'));
                        if(System.IO.File.Exists(oldImagePath)) 
                        {
                            System.IO.File.Delete(oldImagePath);
                        }
                    }

                    using (var fileStreams = new FileStream(Path.Combine(uploads, fileName + extension), FileMode.Create))
                    {
                        file.CopyTo(fileStreams);
                    }
                    movieVM.Movie.ImageUrl = @"\images\movies\" + fileName + extension;
                }

                if (movieVM.Movie.Id == 0)
                {
                    _db.Movies.Add(movieVM.Movie);
                    TempData["success"] = "Movie created successfully";
                }
                else
                {
                    _db.Movies.Update(movieVM.Movie);
                    TempData["success"] = "Movie updated successfully";
                }

                    _db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(movieVM);
        }

        #region API CALLS
        [HttpGet]
        public IActionResult GetAll()
        {

            var movieList = _db.Movies
                .Include(u => u.Category)
                .Include(u => u.CoverType)
                .ToList();

            return Json(new { data = movieList });
        }

        [HttpDelete]
        public IActionResult Delete(int? id)
        {
            var movieObj = _db.Movies.Find(id);
            if (movieObj == null)
            {
                return Json(new { success = false, message = "Error while deleting" });
            }

            var oldImagePath = Path.Combine(_hostEnvironment.WebRootPath, movieObj.ImageUrl.TrimStart('\\'));
            if (System.IO.File.Exists(oldImagePath))
            {
                System.IO.File.Delete(oldImagePath);
            }


            _db.Movies.Remove(movieObj);
            _db.SaveChanges();
            return Json(new { success = true, message = "Successfully deleted" });
        }
        #endregion
    }
}
