using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ST2_2026_Team_4_GameLibrary.Models
{

    public class Game
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Заглавието е задължително.")]
        [StringLength(100)]
        public string Title { get; set; } = string.Empty;

        [StringLength(2000)]
        public string? Description { get; set; }

        [Required(ErrorMessage = "Жанрът е задължителен.")]
        [StringLength(50)]
        public string Genre { get; set; } = string.Empty;

        [DataType(DataType.Date)]
        public DateTime ReleaseDate { get; set; }

        [Required(ErrorMessage = "Разработчикът е задължителен.")]
        [StringLength(100)]
        public string Developer { get; set; } = string.Empty;

        public string? ImagePath{ get; set; }

        [NotMapped]
        public IFormFile? CoverImage { get; set; }


        public List<Library> Libraries { get; set; } = new();
    }
}
