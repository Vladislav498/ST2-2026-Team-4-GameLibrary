
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using ST2_2026_Team_4_GameLibrary.Models;

namespace ST2_2026_Team_4_GameLibrary.Controllers
{
    [Authorize]
    public class UserController : Controller
    {
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;

        public UserController(
            UserManager<User> userManager,
            SignInManager<User> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }

        [HttpGet]
        public async Task<IActionResult> Details(string? id)
        {
            User? user;

            if (string.IsNullOrEmpty(id))
            {
                user = await _userManager.GetUserAsync(User);
            }
            else
            {
                user = await _userManager.FindByIdAsync(id);
            }

            if (user == null)
                return NotFound();

            return View(user);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteProfile(
            string id,
            string currentPassword)
        {
            if (_userManager.GetUserId(User) != id)
                return Forbid();

            var user = await _userManager.FindByIdAsync(id);

            if (user == null)
                return NotFound();

            if (string.IsNullOrWhiteSpace(currentPassword) ||
                !await _userManager.CheckPasswordAsync(user, currentPassword))
            {
                TempData["ProfileMessage"] = "Грешна парола.";
                return RedirectToAction(nameof(Details), new { id });
            }

            var result = await _userManager.DeleteAsync(user);

            if (!result.Succeeded)
            {
                TempData["ProfileMessage"] =
                    string.Join(" ", result.Errors.Select(e => e.Description));

                return RedirectToAction(nameof(Details), new { id });
            }

            await _signInManager.SignOutAsync();

            return RedirectToAction("Index", "Home");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ChangeName(
            string id,
            string userName)
        {
            if (_userManager.GetUserId(User) != id)
                return Forbid();

            var user = await _userManager.FindByIdAsync(id);

            if (user == null)
                return NotFound();

            if (string.IsNullOrWhiteSpace(userName))
            {
                SetProfileError(
                    "Името е задължително.",
                    "nameForm");

                return RedirectToAction(nameof(Details), new { id });
            }

            var result = await _userManager.SetUserNameAsync(
                user, userName.Trim());

            if (!result.Succeeded)
            {
                SetProfileError(
                    string.Join(" ", result.Errors.Select(e => e.Description)),
                    "nameForm");

                return RedirectToAction(nameof(Details), new { id });
            }

            TempData["ProfileMessage"] = "Името е променено успешно.";

            return RedirectToAction(nameof(Details), new { id });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ChangeEmail(
            string id,
            string email,
            string currentPassword)
        {
            if (_userManager.GetUserId(User) != id)
                return Forbid();

            var user = await _userManager.FindByIdAsync(id);

            if (user == null)
                return NotFound();

            if (string.IsNullOrWhiteSpace(email))
            {
                SetProfileError(
                    "Имейлът е задължителен.",
                    "emailForm");

                return RedirectToAction(nameof(Details), new { id });
            }

            if (string.IsNullOrWhiteSpace(currentPassword) ||
                !await _userManager.CheckPasswordAsync(user, currentPassword))
            {
                SetProfileError(
                    "Текущата парола е неправилна.",
                    "emailForm");

                return RedirectToAction(nameof(Details), new { id });
            }

            var result = await _userManager.SetEmailAsync(
                user, email.Trim());

            if (!result.Succeeded)
            {
                SetProfileError(
                    string.Join(" ", result.Errors.Select(e => e.Description)),
                    "emailForm");

                return RedirectToAction(nameof(Details), new { id });
            }

            TempData["ProfileMessage"] = "Имейлът е променен успешно.";

            return RedirectToAction(nameof(Details), new { id });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ChangePassword(
            string id,
            string currentPassword,
            string newPassword,
            string confirmPassword)
        {
            if (_userManager.GetUserId(User) != id)
                return Forbid();

            var user = await _userManager.FindByIdAsync(id);

            if (user == null)
                return NotFound();

            if (string.IsNullOrWhiteSpace(currentPassword))
            {
                SetProfileError(
                    "Текущата парола е задължителна.",
                    "passwordForm");

                return RedirectToAction(nameof(Details), new { id });
            }

            if (string.IsNullOrWhiteSpace(newPassword))
            {
                SetProfileError(
                    "Новата парола е задължителна.",
                    "passwordForm");

                return RedirectToAction(nameof(Details), new { id });
            }

            if (newPassword != confirmPassword)
            {
                SetProfileError(
                    "Паролите не съвпадат.",
                    "passwordForm");

                return RedirectToAction(nameof(Details), new { id });
            }

            var result = await _userManager.ChangePasswordAsync(
                user, currentPassword, newPassword);

            if (!result.Succeeded)
            {
                SetProfileError(
                    string.Join(" ", result.Errors.Select(e => e.Description)),
                    "passwordForm");

                return RedirectToAction(nameof(Details), new { id });
            }

            await _signInManager.RefreshSignInAsync(user);

            TempData["ProfileMessage"] = "Паролата е променена успешно.";

            return RedirectToAction(nameof(Details), new { id });
        }

        private void SetProfileError(string message, string formName)
        {
            TempData["ProfileMessage"] = message;
            TempData["OpenProfileForm"] = formName;
        }


        [AllowAnonymous]
        [AcceptVerbs("GET", "POST")]
        public async Task<IActionResult> IsNameAvailable(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
                return Json(true); // [Required] handles the empty case

            var existing = await _userManager.FindByNameAsync(name.Trim());
            var currentUserId = _userManager.GetUserId(User);

            return Json(existing == null || existing.Id == currentUserId);
        }
    }
}
