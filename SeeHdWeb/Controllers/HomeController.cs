using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SeeHdWeb.Data;
using SeeHdWeb.Models;
using System.Diagnostics;
using System.Security.Claims;

namespace SeeHdWeb.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ApplicationDbContext _db;
        private readonly TmdbService _tmdb;

        public HomeController(ILogger<HomeController> logger, ApplicationDbContext db, TmdbService tmdb)
        {
            _logger = logger;
            _db = db;
            _tmdb = tmdb;
        }

        // Index Get Action Method
        public IActionResult Index()
        {
            var movieList = _db.Movies
                .Include(u => u.Category)
                .Include(u => u.CoverType)
                .ToList();

            return View(movieList);
        }

        // MovieDetails GET: Public
        public IActionResult MovieDetails(int movieId)
        {
            var movie = _db.Movies
                .Include(u => u.Category)
                .Include(u => u.CoverType)
                .FirstOrDefault(u => u.Id == movieId);

            if (movie == null)
                return NotFound();

            var viewModel = new WhileList
            {
                MoviesId = movieId,
                Movies = movie,
                IsAdded = false
            };

            if (User.Identity.IsAuthenticated)
            {
                var claim = (User.Identity as ClaimsIdentity)?.FindFirst(ClaimTypes.NameIdentifier);
                if (claim != null)
                {
                    var userId = claim.Value;
                    var exists = _db.WhileLists
                        .Any(w => w.ApplicationUserId == userId && w.MoviesId == movieId);

                    viewModel.IsAdded = exists;
                }
            }

            return View(viewModel);
        }

        // MovieDetails POST: Logged-in users only
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public IActionResult MovieDetails(WhileList whileList)
        {
            if (whileList == null || whileList.MoviesId == 0)
                return BadRequest();

            var claim = (User.Identity as ClaimsIdentity)?.FindFirst(ClaimTypes.NameIdentifier);
            if (claim == null)
                return Unauthorized();

            whileList.ApplicationUserId = claim.Value;

            var existing = _db.WhileLists.FirstOrDefault(w =>
                w.ApplicationUserId == whileList.ApplicationUserId &&
                w.MoviesId == whileList.MoviesId);

            if (existing == null)
            {
                _db.WhileLists.Add(whileList);
                _db.SaveChanges();
                whileList.IsAdded = true;
            }
            else
            {
                whileList.IsAdded = true; // Already exists
            }

            whileList.Movies = _db.Movies
                .Include(m => m.Category)
                .Include(m => m.CoverType)
                .FirstOrDefault(m => m.Id == whileList.MoviesId);

            return View(whileList);
        }

        // WhileList Page for logged-in users
        [Authorize]
        public IActionResult WhileList()
        {
            var claim = (User.Identity as ClaimsIdentity)?.FindFirst(ClaimTypes.NameIdentifier);
            if (claim == null)
                return Unauthorized();

            var userId = claim.Value;

            var userWhileList = _db.WhileLists
                .Include(w => w.Movies)
                    .ThenInclude(m => m.Category)
                .Include(w => w.Movies)
                    .ThenInclude(m => m.CoverType)
                .Where(w => w.ApplicationUserId == userId)
                .ToList();

            return View(userWhileList);
        }

        // Delete from WhileList
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public IActionResult Delete(int id)
        {
            var item = _db.WhileLists.FirstOrDefault(w => w.Id == id);
            if (item == null)
                return NotFound();

            _db.WhileLists.Remove(item);
            _db.SaveChanges();

            return RedirectToAction("WhileList");
        }

        // Search
        [HttpGet]
        public async Task<IActionResult> Search(string term)
        {
            if (string.IsNullOrEmpty(term))
            {
                return RedirectToAction("Index");
            }

            var results = await _tmdb.SearchMoviesAsync(term);
            ViewBag.SearchTerm = term;
            return View(results.results);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
