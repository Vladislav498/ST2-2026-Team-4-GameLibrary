using System;
using System.Collections.Generic;
using System.Linq;
using ST2_2026_Team_4_GameLibrary.Data;
using ST2_2026_Team_4_GameLibrary.Models;

namespace ST2_2026_Team_4_GameLibrary.Services
{

    //https://www.c-sharpcorner.com/article/understanding-iservicescopefactory-in-net-when-why-and-how-to-use-it-with-r
    
    public sealed class FeaturedGamesSingleton
    {
        private readonly IServiceScopeFactory _scopeFactory;
        private static readonly Random _random = new Random();

        public FeaturedGamesSingleton(IServiceScopeFactory scopeFactory)
        {
            _scopeFactory = scopeFactory;
        }

        public List<Game> GetFeaturedGames(int count = 4)
        {
            // Create a fresh scope and get a new context for this operation
            using (var scope = _scopeFactory.CreateScope())
            {
                var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();
                var allGames = GamesSeed.Initialize(context);

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
}