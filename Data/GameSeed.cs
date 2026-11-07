using Microsoft.EntityFrameworkCore;
using ST2_2026_Team_4_GameLibrary.Data;
using System;
using System.Collections.Generic;
using System.Linq;
namespace ST2_2026_Team_4_GameLibrary.Models
{
    
        public static class GameSeed
        {
            public static List<Game> Initialize(AppDbContext context)
            {
                if (context == null) throw new ArgumentNullException(nameof(context));

               
                context.Database.EnsureCreated();

              
                var games = context.Games
                                   .AsNoTracking()
                                   .ToList();

                return games ?? new List<Game>();
            }
        }
    }
