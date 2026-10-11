using ST2_2026_Team_4_GameLibrary.Models;

namespace ST2_2026_Team_4_GameLibrary.Data
{
    //Dobavqm nqkvi igri v bazata
    public static class GamesSeed
    {

        public static void Initialize(AppDbContext context)
        {
            if (context.Games.Any())
            {
                return;
            }

            var games = new List<Game>
            {
                new Game
                {
                    Title = "The Witcher 3: Wild Hunt (Remastered)",
                    Description = "An open-world fantasy RPG.",
                    Genre = "RPG",
                    ReleaseDate = new DateTime(2015, 5, 19),
                    Developer = "CD Projekt Red",
                    CoverImage = "/images/thewitcher3remastered.avif"
                },
                new Game
                {
                    Title = "Minecraft",
                    Description = "Explore, build, and survive in a block-based world.",
                    Genre = "Sandbox",
                    ReleaseDate = new DateTime(2011, 11, 18),
                    Developer = "Mojang Studios",
                    CoverImage = "/images/minecraft.avif"
                },
                new Game
                {
                    Title = "Cyberpunk 2077",
                    Description = "An open-world RPG set in Night City.",
                    Genre = "RPG",
                    ReleaseDate = new DateTime(2020, 12, 10),
                    Developer = "CD Projekt Red",
                    CoverImage = "/images/cyberpunk2077.webp"
                },
                new Game
                {
                Title = "Resident Evil Requiem",
        Description = "Survive a terrifying new chapter in the Resident Evil series.",
        Genre = "Survival Horror",
        ReleaseDate = new DateTime(2026, 2, 27),
        Developer = "Capcom",
        CoverImage = "/images/RE9.webp"
                },
                new Game
                {
                     Title = "Hollow Knight: Silksong",
                      Description = "Play as Hornet in a dangerous kingdom filled with enemies and secrets.",
                      Genre = "Metroidvania",
                    ReleaseDate = new DateTime(2025, 9, 4),
                    Developer = "Team Cherry",
                        CoverImage = "/images/hksilksong.webp"
                }

            };

            context.Games.AddRange(games);
            context.SaveChanges();
        }
    
}
}

