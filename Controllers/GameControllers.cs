
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ST2_2026_Team_4_GameLibrary.Models;
using ST2_2026_Team_4_GameLibrary.Data;
using ST2_2026_Team_4_GameLibrary.Services;

namespace ST2_2026_Team_4_GameLibrary.Controllers
{
    public class GameController : Controller
    {
        private readonly AppDbContext _context;
        private readonly IWebHostEnvironment _environment;
        private readonly IGeminiQueryService _geminiQueryService;
        private readonly GameQueryExecutor _gameQueryExecutor;

        public GameController(
            AppDbContext context,
            IWebHostEnvironment environment,
            IGeminiQueryService geminiQueryService,
            GameQueryExecutor gameQueryExecutor)
        {
            _context = context;
            _environment = environment;
            _geminiQueryService = geminiQueryService;
            _gameQueryExecutor = gameQueryExecutor;
        }

        // GET: Game/Index
        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> Index(
            string? genre,
            string? aiQuery,
            CancellationToken cancellationToken)
        {
            var allGenres = await _context.Games
     .AsNoTracking()
     .Select(g => g.Genre)
     .ToListAsync(cancellationToken);

            ViewBag.Genres = allGenres
                .Where(g => !string.IsNullOrWhiteSpace(g))
                .SelectMany(g => g.Split(',', StringSplitOptions.TrimEntries))
                .Where(g => !string.IsNullOrWhiteSpace(g))
                .Distinct(StringComparer.OrdinalIgnoreCase)
                .OrderBy(g => g)
                .ToList();

            ViewBag.SelectedGenre = genre;
            ViewBag.AiQuery = aiQuery;
            ViewBag.AiError = null;

            IQueryable<Game> gamesQuery = _context.Games.AsNoTracking();

            if (!string.IsNullOrWhiteSpace(aiQuery))
            {
                var geminiResult = await _geminiQueryService
                    .GenerateJsonQueryAsync(
                        aiQuery.Trim(),
                        cancellationToken);

                if (!geminiResult.Success ||
                    string.IsNullOrWhiteSpace(geminiResult.Query))
                {
                    ViewBag.AiError = geminiResult.ErrorMessage
                        ?? "AI търсенето не беше успешно.";

                    var fallbackGames = await gamesQuery
                        .OrderBy(g => g.Title)
                        .ToListAsync(cancellationToken);

                    return View(fallbackGames);
                }

                try
                {
                    var jsonQuery =
                        System.Text.Json.JsonSerializer.Deserialize<JSONQuery>(
                            geminiResult.Query);

                    if (!GameQueryValidator.TryValidate(
                        jsonQuery, out var validationError))
                    {
                        ViewBag.AiError =
                            $"Невалидна AI заявка: {validationError}";

                        var fallbackGames = await gamesQuery
                            .OrderBy(g => g.Title)
                            .ToListAsync(cancellationToken);

                        return View(fallbackGames);
                    }

                    var games = await _gameQueryExecutor.ExecuteAsync(
                        jsonQuery!,
                        cancellationToken);

                    ViewBag.AiSearchActive = true;

                    return View(games);
                }
                catch (System.Text.Json.JsonException)
                {
                    ViewBag.AiError =
                        "AI върна отговор, който не може да бъде обработен.";
                }
                catch (ArgumentException ex)
                {
                    ViewBag.AiError = ex.Message;
                }
            }

            if (!string.IsNullOrWhiteSpace(genre))
            {
                gamesQuery = gamesQuery.Where(g =>
                    ("," + g.Genre.Replace(" ", "") + ",")
                    .Contains("," + genre.Replace(" ", "") + ","));
            }

            var result = await gamesQuery
                .OrderBy(g => g.Title)
                .ToListAsync(cancellationToken);

            return View(result);
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
                    if (game.CoverImage.Length > 5 * 1024 * 1024)
                    {
                        ModelState.AddModelError(
                            "CoverImage",
                            "Image must be 5 MB or smaller.");

                        return View(game);
                    }

                    var extension = Path.GetExtension(
                        game.CoverImage.FileName).ToLowerInvariant();

                    var allowedExtensions =
                        new[] { ".jpg", ".jpeg", ".png", ".webp", ".avif" };

                    if (!allowedExtensions.Contains(extension))
                    {
                        ModelState.AddModelError(
                            "CoverImage",
                            "Само изображения с разширения .JPG, .PNG, .WEBP, .AVIF са позволени.");

                        return View(game);
                    }

                    var uploadsFolder = Path.Combine(
                        _environment.WebRootPath, "images", "games");

                    Directory.CreateDirectory(uploadsFolder);

                    var fileName = $"{Guid.NewGuid()}{extension}";
                    var filePath = Path.Combine(uploadsFolder, fileName);

                    using (var stream = new FileStream(
                        filePath, FileMode.Create))
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

            var existing = await _context.Games.FindAsync(id);

            if (existing == null)
            {
                return NotFound();
            }

            if (!ModelState.IsValid)
            {
                return View(game);
            }

         
            existing.Title = game.Title;
            existing.Description = game.Description;
            existing.Genre = game.Genre;
            existing.ReleaseDate = game.ReleaseDate;
            existing.Developer = game.Developer;

            string? oldImagePath = existing.ImagePath;

      



            if (game.CoverImage != null && game.CoverImage.Length > 0)
            {
                var allowedExtensions = new[] { ".jpg", ".jpeg", ".png", ".webp" };
                var extension = Path.GetExtension(game.CoverImage.FileName)
                    .ToLowerInvariant();

                if (!allowedExtensions.Contains(extension))
                {
                    ModelState.AddModelError(
                        "CoverImage",
                        "Моля, качете JPG, PNG или WEBP изображение.");

                    return View(game);
                }

                if (game.CoverImage.Length > 5 * 1024 * 1024)
                {
                    ModelState.AddModelError(
                        "CoverImage",
                        "Изображението не трябва да надвишава 5 MB.");

                    return View(game);
                }

                var uploadsFolder = Path.Combine(
                    Directory.GetCurrentDirectory(),
                    "wwwroot",
                    "images",
                    "games");

                Directory.CreateDirectory(uploadsFolder);

                var fileName = $"{Guid.NewGuid()}{extension}";
                var filePath = Path.Combine(uploadsFolder, fileName);

                await using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await game.CoverImage.CopyToAsync(stream);
                }

                existing.ImagePath = "/images/games/" + fileName;
            }

            await _context.SaveChangesAsync();

            
            if (game.CoverImage != null &&
                !string.IsNullOrEmpty(oldImagePath))
            {
                var oldFileName = Path.GetFileName(oldImagePath);

                var oldFilePath = Path.Combine(
                    Directory.GetCurrentDirectory(),
                    "wwwroot",
                    "images",
                    "games",
                    oldFileName);

                if (System.IO.File.Exists(oldFilePath))
                {
                    System.IO.File.Delete(oldFilePath);
                }
            }

            return RedirectToAction(nameof(Index));
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
