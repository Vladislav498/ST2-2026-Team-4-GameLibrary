using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using ST2_2026_Team_4_GameLibrary.Models;

namespace ST2_2026_Team_4_GameLibrary.Controllers
{
    public class LibraryController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult Details(int id)
        {
            return View();
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Library library)
        {
            if (ModelState.IsValid)
            {
                return RedirectToAction(nameof(Index));
            }

            return View(library);
        }

        public IActionResult Edit(int id)
        {
            return View();
        }

        public IActionResult Delete(int id)
        {
            return View();
        }
    }
}
