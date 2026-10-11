using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using ST2_2026_Team_4_GameLibrary.Models;

namespace ST2_2026_Team_4_GameLibrary.Controllers
{
    public class GameController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Game game)
        {
            if (ModelState.IsValid)
            {
                return RedirectToAction(nameof(Index));
            }

            return View(game);
        }

        public IActionResult Details(int id)
        {
            return View();
        }

        public IActionResult Edit(int id)
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int id, Game game)
        {
            if (id != game.Id)
            {
                return BadRequest();
            }

            if (ModelState.IsValid)
            {
                return RedirectToAction(nameof(Index));
            }

            return View(game);
        }

        public IActionResult Delete(int id)
        {
            return View();
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            return RedirectToAction(nameof(Index));
        }
    }
}
