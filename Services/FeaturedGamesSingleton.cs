using System;
using System.Collections.Generic;
using System.Linq;
using ST2_2026_Team_4_GameLibrary.Data;
using ST2_2026_Team_4_GameLibrary.Models;

namespace ST2_2026_Team_4_GameLibrary.Services
{
    public sealed class FeaturedGamesSingleton
    {
        private static  AppDbContext? _context = null;
        private static readonly Random _random = new Random();
        private static FeaturedGamesSingleton? instance = null;
        private static object _lock = new object();

        private FeaturedGamesSingleton()
        {
           
        }

        public static FeaturedGamesSingleton GetSingleInstance(AppDbContext context)
        {
            if (instance == null)
            {
                lock (_lock)
                {
                    if (instance == null)

                    {

                        _context = context;
                        instance = new FeaturedGamesSingleton();
                    }
                }
            }
            return instance;
        }

        public static  List<Game> GetFeaturedGames(int count = 4)
        {
            // Get all games from database using GamesSeed
            var allGames = GamesSeed.Initialize(_context!);

            if (allGames == null || !allGames.Any())
            {
                return new List<Game>();
            }

            // Ensure we don't request more games than available
            var takeCount = Math.Min(count, allGames.Count);

            // Use Fisher-Yates shuffle to ensure unique selection
            var shuffledGames = new List<Game>(allGames);
            for (int i = shuffledGames.Count - 1; i > 0; i--)
            {
                int j = _random.Next(i + 1);
                // Swap elements
                (shuffledGames[i], shuffledGames[j]) = (shuffledGames[j], shuffledGames[i]);
            }

            return shuffledGames.Take(takeCount).ToList();
        }
    }
}