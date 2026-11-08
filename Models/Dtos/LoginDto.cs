using System.ComponentModel.DataAnnotations;

namespace ST2_2026_Team_4_GameLibrary.Models
{
    public class LoginDto
    {
        [Display(Name = "Имейл")]
        [Required(ErrorMessage = "Имейлът е задължителен")]
        [EmailAddress(ErrorMessage = "Невалиден имейл")]
        public string Email { get; set; }

        [Display(Name = "Парола")]
        [Required(ErrorMessage = "Паролата е задължителна")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Display(Name = "Запомни ме")]
        public bool RememberMe { get; set; }
    }
}
