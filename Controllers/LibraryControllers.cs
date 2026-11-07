
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ST2_2026_Team_4_GameLibrary.Data;
using ST2_2026_Team_4_GameLibrary.Models;

namespace ST2_2026_Team_4_GameLibrary.Controllers
{
    [Authorize]
    public class LibraryController : Controller
    {
        private readonly AppDbContext _context;
        private readonly UserManager<User> _userManager;

        public LibraryController(
            AppDbContext context,
            UserManager<User> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // Показва личната библиотека на влезлия потребител
        public async Task<IActionResult> Index()
        {
            var userId = _userManager.GetUserId(User);

            var games = await _context.Libraries
                .Where(l => l.UserId == userId)
                .Include(l => l.Game)
                .Select(l => l.Game)
                .AsNoTracking()
                .ToListAsync();

            return View(games);
        }

        // Добавя игра в личната библиотека
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Add(int gameId)
        {
            var userId = _userManager.GetUserId(User);

            var gameExists = await _context.Games
                .AnyAsync(g => g.Id == gameId);

            if (!gameExists)
            {
                return NotFound();
            }

            var alreadyAdded = await _context.Libraries
                .AnyAsync(l => l.UserId == userId &&
                               l.GameId == gameId);

            if (!alreadyAdded)
            {
                var libraryEntry = new Library
                {
                    UserId = userId!,
                    GameId = gameId
                };

                _context.Libraries.Add(libraryEntry);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Index));
        }

        // Премахва игра от личната библиотека
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Remove(int gameId)
        {
            var userId = _userManager.GetUserId(User);

            var libraryEntry = await _context.Libraries
                .FirstOrDefaultAsync(l =>
                    l.UserId == userId &&
                    l.GameId == gameId);

            if (libraryEntry != null)
            {
                _context.Libraries.Remove(libraryEntry);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Index));
        }
    }
}
