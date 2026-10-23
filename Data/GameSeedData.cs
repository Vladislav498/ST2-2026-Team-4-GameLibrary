using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
namespace ST2_2026_Team_4_GameLibrary.Data
{
    public static class GameSeedData
    {
        public static List<Game> GetGames()
        {
            return new List<Game>
            {
                new Game
                {
                    Title = "The Witcher 3: Wild Hunt",
                    Description = "Open-world RPG, в която играчът поема ролята на Гералт от Ривия.",
                    Genre = "RPG",
                    ReleaseDate = new DateTime(2015, 5, 19),
                    Developer = "CD Projekt Red",
                    CoverImage = "images/witcher3.jpg"
                },

                new Game
                {
                    Title = "Minecraft",
                    Description = "Sandbox игра за строене, изследване и оцеляване.",
                    Genre = "Sandbox",
                    ReleaseDate = new DateTime(2011, 11, 18),
                    Developer = "Mojang Studios",
                    CoverImage = "images/minecraft.jpg"
                },

                new Game
                {
                    Title = "Cyberpunk 2077",
                    Description = "Open-world RPG, чието действие се развива в Night City.",
                    Genre = "RPG",
                    ReleaseDate = new DateTime(2020, 12, 10),
                    Developer = "CD Projekt Red",
                    CoverImage = "images/cyberpunk2077.jpg"
                },

                new Game
                {
                    Title = "Grand Theft Auto V",
                    Description = "Open-world екшън игра, развиваща се в Los Santos.",
                    Genre = "Action",
                    ReleaseDate = new DateTime(2013, 9, 17),
                    Developer = "Rockstar North",
                    CoverImage = "images/gta5.jpg"
                },

                new Game
                {
                    Title = "Red Dead Redemption 2",
                    Description = "Приключенска игра с отворен свят, развиваща се в Дивия запад.",
                    Genre = "Action",
                    ReleaseDate = new DateTime(2018, 10, 26),
                    Developer = "Rockstar Studios",
                    CoverImage = "images/rdr2.jpg"
                },

                new Game
                {
                    Title = "Forza Horizon 5",
                    Description = "Състезателна игра с отворен свят, развиваща се в Мексико.",
                    Genre = "Racing",
                    ReleaseDate = new DateTime(2021, 11, 9),
                    Developer = "Playground Games",
                    CoverImage = "images/forza5.jpg"
                },

                new Game
                {
                    Title = "Elden Ring",
                    Description = "Action RPG с голям отворен свят и предизвикателни битки.",
                    Genre = "Action RPG",
                    ReleaseDate = new DateTime(2022, 2, 25),
                    Developer = "FromSoftware",
                    CoverImage = "images/eldenring.jpg"
                },

                new Game
                {
                    Title = "God of War Ragnarök",
                    Description = "Приключенска екшън игра за Кратос и Атреус.",
                    Genre = "Action",
                    ReleaseDate = new DateTime(2022, 11, 9),
                    Developer = "Santa Monica Studio",
                    CoverImage = "images/godofwar.jpg"
                },

                new Game
                {
                    Title = "Hogwarts Legacy",
                    Description = "Приключенска RPG игра във вселената на Harry Potter.",
                    Genre = "RPG",
                    ReleaseDate = new DateTime(2023, 2, 10),
                    Developer = "Avalanche Software",
                    CoverImage = "images/hogwartslegacy.jpg"
                },

                new Game
                {
                    Title = "Baldur's Gate 3",
                    Description = "Фентъзи RPG с избори, тактически битки и кооперативна игра.",
                    Genre = "RPG",
                    ReleaseDate = new DateTime(2023, 8, 3),
                    Developer = "Larian Studios",
                    CoverImage = "images/baldursgate3.jpg"
                },

                new Game
                {
                    Title = "Resident Evil 4",
                    Description = "Survival horror игра с екшън и решаване на загадки.",
                    Genre = "Horror",
                    ReleaseDate = new DateTime(2023, 3, 24),
                    Developer = "Capcom",
                    CoverImage = "images/residentevil4.jpg"
                },

                new Game
                {
                    Title = "Hades",
                    Description = "Roguelike екшън игра, базирана на гръцката митология.",
                    Genre = "Roguelike",
                    ReleaseDate = new DateTime(2020, 9, 17),
                    Developer = "Supergiant Games",
                    CoverImage = "images/hades.jpg"
                },

                new Game
                {
                    Title = "Stardew Valley",
                    Description = "Симулатор за ферма с изследване, занаяти и взаимоотношения.",
                    Genre = "Simulation",
                    ReleaseDate = new DateTime(2016, 2, 26),
                    Developer = "ConcernedApe",
                    CoverImage = "images/stardewvalley.jpg"
                },

                new Game
                {
                    Title = "Grand Theft Auto: San Andreas",
                    Description = "Класическа open-world игра за CJ и неговото завръщане в Los Santos.",
                    Genre = "Action",
                    ReleaseDate = new DateTime(2004, 10, 26),
                    Developer = "Rockstar North",
                    CoverImage = "images/"
                }
            };
        }
    }
}