using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ST2_2026_Team_4_GameLibrary.Models;
using ST2_2026_Team_4_GameLibrary.Services;
using ST2_2026_Team_4_GameLibrary.Data;

namespace ST2_2026_Team_4_GameLibrary.Controllers
{
    public class HomeController : Controller
    {
        private readonly AppDbContext _context;

        public HomeController(AppDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            // Get 4 random featured games
            var featuredGamesSingle = FeaturedGamesSingleton.GetSingleInstance(_context);

            var featuredGames = FeaturedGamesSingleton.GetFeaturedGames(4);
            return View(featuredGames);
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