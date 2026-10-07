namespace ST2_2026_Team_4_GameLibrary.Models
{
    public class Library
    {


        public string UserId { get; set; } = string.Empty;
        public User User { get; set; } = null!;

        public int GameId { get; set; }
        public Game Game { get; set; } = null!;


    }
}
