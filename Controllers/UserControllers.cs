using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using ST2_2026_Team_4_GameLibrary.Models;

namespace ST2_2026_Team_4_GameLibrary.Controllers
{
    public class UserController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Details(string id)
        {
            return View();
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(User user)
        {
            if (ModelState.IsValid)
            {
                return RedirectToAction(nameof(Index));
            }

            return View(user);
        }

        public IActionResult Edit(string id)
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(string id, User user)
        {
            if (id != user.Id)
            {
                return BadRequest();
            }

            if (ModelState.IsValid)
            {
                return RedirectToAction(nameof(Index));
            }

            return View(user);
        }

        public IActionResult Delete(string id)
        {
            return View();
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(string id)
        {
            return RedirectToAction(nameof(Index));
        }
    }
}
