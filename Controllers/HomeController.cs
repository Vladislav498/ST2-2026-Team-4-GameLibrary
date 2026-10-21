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
        private readonly FeaturedGamesSingleton _featuredGamesSingleton;

        public HomeController(AppDbContext context, FeaturedGamesSingleton featuredGamesSingleton)
        {
            _context = context;
            _featuredGamesSingleton = featuredGamesSingleton;
        }

        public IActionResult Index()
        {
           
            var featuredGames = _featuredGamesSingleton.GetFeaturedGames(4);
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