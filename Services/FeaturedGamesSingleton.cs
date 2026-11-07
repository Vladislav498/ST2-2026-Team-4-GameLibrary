using Microsoft.EntityFrameworkCore;
using ST2_2026_Team_4_GameLibrary.Data;
using ST2_2026_Team_4_GameLibrary.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ST2_2026_Team_4_GameLibrary.Services
{

    //https://www.c-sharpcorner.com/article/understanding-iservicescopefactory-in-net-when-why-and-how-to-use-it-with-r
    
    public sealed class FeaturedGamesSingleton
    {
        private readonly IDbContextFactory<AppDbContext> _contextFactory;
        private static readonly Random _random = new Random();

        public FeaturedGamesSingleton(IDbContextFactory<AppDbContext> contextFactory)
        {
            _contextFactory = contextFactory;
        }

        public List<Game> GetFeaturedGames(int count = 4)
        {

            using (var scope = _contextFactory.CreateDbContext())
            {
                using var context = _contextFactory.CreateDbContext();
                var allGames = GameSeed.Initialize(context);

                if (allGames == null || !allGames.Any())
                {
                    return new List<Game>();
                }


                var takeCount = Math.Min(count, allGames.Count);


                var shuffledGames = new List<Game>(allGames);
                for (int i = shuffledGames.Count - 1; i > 0; i--)
                {
                    int j = _random.Next(i + 1);

                    (shuffledGames[i], shuffledGames[j]) = (shuffledGames[j], shuffledGames[i]);
                }

                return shuffledGames.Take(takeCount).ToList();
            }






    



}
    }
}