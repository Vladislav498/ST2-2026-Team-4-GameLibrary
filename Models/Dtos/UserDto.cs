using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace ST2_2026_Team_4_GameLibrary.Models
{
    public class UserDto
    {
        public const string NamePattern = "^[A-Za-z\u0400-\u04FF][A-Za-z\u0400-\u04FF '.-]{1,39}$";

        [Display(Name = "Име")]
        [Required(ErrorMessage = "Името е задължително.")]
        [StringLength(40, MinimumLength = 2, ErrorMessage = "Името може да съдържа от 2 до 40 символа.")]
        [RegularExpression(NamePattern, ErrorMessage = "Позволени са само букви, интервал, тире, апостроф и точка.")]
        [Remote(action: "IsNameAvailable", controller: "User", ErrorMessage = "Името се използва от друг потребител.")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Email is required")]
        [EmailAddress(ErrorMessage = "Invalid email address")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Password is required")]
        [StringLength(100, ErrorMessage = "Password must be at least 6 characters", MinimumLength = 6)]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "Passwords do not match")]
        public string? ConfirmPassword { get; set; }

        [Display(Name = "Remember me?")]
        public bool RememberMe { get; set; }
    }
}
