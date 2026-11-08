using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace ST2_2026_Team_4_GameLibrary.Models
{
    public class RegisterDto

    {
        public const string NamePattern = "^[A-Za-z\u0400-\u04FF][A-Za-z\u0400-\u04FF '.-]{1,39}$";

        [Display(Name = "Име")]
        [Required(ErrorMessage = "Името е задължително.")]
        [StringLength(40, MinimumLength = 2, ErrorMessage = "Името може да съдържа от 2 до 40 символа.")]
        [RegularExpression(NamePattern, ErrorMessage = "Позволени са само букви, интервал, тире, апостроф и точка.")]
        [Remote(action: "IsNameAvailable", controller: "User", ErrorMessage = "Името се използва от друг потребител.")]
        public string Name { get; set; }

        [Display(Name = "Имейл")]
        [Required(ErrorMessage = "Имейлът е задължителен")]
        [EmailAddress(ErrorMessage = "Невалиден имейл")]
        public string Email { get; set; }


        [Display(Name = "Парола")]
        [Required(ErrorMessage = "Паролата е задължителна")]
        [StringLength(100, ErrorMessage = "Паролата трябва да съдържа главна буква, символ и поне 6 знака", MinimumLength = 6)]
        [DataType(DataType.Password)]
        public string Password { get; set; }



        [Display(Name = "Потвърди парола")]
        [Required(ErrorMessage = "Моля потвърдете парола")]
        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "Паролите не съвпадат")]
        public string ConfirmPassword { get; set; }
    }
}
