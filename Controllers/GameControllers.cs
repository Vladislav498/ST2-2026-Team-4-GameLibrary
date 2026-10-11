
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ST2_2026_Team_4_GameLibrary.Models;
using ST2_2026_Team_4_GameLibrary.Data;

namespace ST2_2026_Team_4_GameLibrary.Controllers
{
    public class GameController : Controller
    {
        private readonly AppDbContext _context;

        public GameController(AppDbContext context)
        {
            _context = context;
        }

        // filtur po janr
        public async Task<IActionResult> Index(string? genre)
        {
            var gamesQuery = _context.Games.AsQueryable();

            if (!string.IsNullOrWhiteSpace(genre))
            {
                gamesQuery = gamesQuery.Where(g => g.Genre == genre);
            }

            ViewBag.Genres = await _context.Games
                .Select(g => g.Genre)
                .Distinct()
                .OrderBy(g => g)
                .ToListAsync();

            ViewBag.SelectedGenre = genre;

            var games = await gamesQuery
                .OrderBy(g => g.Title)
                .ToListAsync();

            return View(games);
        }

    
        public IActionResult Create()
        {
            return View();
        }

       
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Game game)
        {
            if (ModelState.IsValid)
            {
                _context.Games.Add(game);
                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }

            return View(game);
        }

    
        public async Task<IActionResult> Details(int id)
        {
            var game = await _context.Games
                .FirstOrDefaultAsync(g => g.Id == id);

            if (game == null)
            {
                return NotFound();
            }

            return View(game);
        }

        public async Task<IActionResult> Edit(int id)
        {
            var game = await _context.Games.FindAsync(id);

            if (game == null)
            {
                return NotFound();
            }

            return View(game);
        }

    
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Game game)
        {
            if (id != game.Id)
            {
                return BadRequest();
            }

            if (ModelState.IsValid)
            {
                _context.Games.Update(game);
                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }

            return View(game);
        }

        
        public async Task<IActionResult> Delete(int id)
        {
            var game = await _context.Games
                .FirstOrDefaultAsync(g => g.Id == id);

            if (game == null)
            {
                return NotFound();
            }

            return View(game);
        }

    
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var game = await _context.Games.FindAsync(id);

            if (game != null)
            {
                _context.Games.Remove(game);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Index));
        }
    }
}
