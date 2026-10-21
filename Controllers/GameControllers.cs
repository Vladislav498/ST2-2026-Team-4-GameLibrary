
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ST2_2026_Team_4_GameLibrary.Models;
using ST2_2026_Team_4_GameLibrary.Data;

namespace ST2_2026_Team_4_GameLibrary.Controllers
{
    public class GameController : Controller
    {
        private readonly AppDbContext _context;
        private readonly IWebHostEnvironment _environment;

        public GameController(AppDbContext context,IWebHostEnvironment environment)
        {
            _context = context;
            _environment = environment;
        }

        // GET: Game/Index
        [HttpGet]
        [AllowAnonymous]
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

        // GET: Game/Create
        [HttpGet]
        [Authorize(Roles = "Admin")]
        public IActionResult Create()
        {
            return View();
        }

        // POST: Game/Create
        [HttpPost]
        [Authorize(Roles = "Admin")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Game game)
        {

            if (ModelState.IsValid)
            {
                

                if (game.CoverImage != null && game.CoverImage.Length > 0)
                {
                    // Limit file size to 5 MB
                    if (game.CoverImage.Length > 5 * 1024 * 1024)
                    {
                        ModelState.AddModelError(
                            "ImageFile", "Image must be 5 MB or smaller.");

                        return View(game);
                    }

                    // Validate the extension
                    var extension = Path.GetExtension(game.CoverImage.FileName)
                        .ToLowerInvariant();

                    var allowedExtensions = new[] { ".jpg", ".jpeg", ".png", ".webp" };

                    if (!allowedExtensions.Contains(extension))
                    {
                        ModelState.AddModelError(
                            "CoverImage", "Only JPG, PNG and WEBP images are allowed.");

                        return View(game);
                    }

                    var uploadsFolder = Path.Combine(
                        _environment.WebRootPath, "images", "games");

                    Directory.CreateDirectory(uploadsFolder);

                    // Generate a unique filename
                    var fileName = $"{Guid.NewGuid()}{extension}";
                    var filePath = Path.Combine(uploadsFolder, fileName);

                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await game.CoverImage.CopyToAsync(stream);
                    }

                    game.ImagePath = $"/images/games/{fileName}";
                }

                _context.Games.Add(game);
                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }

            return View(game);
        }

        // GET: Game/Details
        [HttpGet]
        [AllowAnonymous]
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

        // GET: Game/Edit
        [HttpGet]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(int id)
        {
            var game = await _context.Games.FindAsync(id);

            if (game == null)
            {
                return NotFound();
            }

            return View(game);
        }

        // POST: Game/Edit
        [HttpPost]
        [Authorize(Roles = "Admin")]
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

        // GET: Game/Delete
        [HttpGet]
        [Authorize(Roles = "Admin")]
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

        // POST: Game/Delete
        [HttpPost, ActionName("Delete")]
        [Authorize(Roles = "Admin")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var game = await _context.Games.FindAsync(id);

            if (game != null)
            {
               
                    

                    if (!string.IsNullOrEmpty(game.ImagePath))
                    {
                        var imagePath = Path.Combine(
                            _environment.WebRootPath,
                            game.ImagePath.TrimStart('/')
                                .Replace('/', Path.DirectorySeparatorChar));

                        if (System.IO.File.Exists(imagePath))
                        {
                            System.IO.File.Delete(imagePath);
                        }
                    }
                    _context.Games.Remove(game);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Index));
        }
    }
}
