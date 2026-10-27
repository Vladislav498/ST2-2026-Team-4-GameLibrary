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

                // Ensure database is created (use Migrate in production if you use migrations)
                context.Database.EnsureCreated();

                // Load games from database. Use AsNoTracking for read-only performance.
                var games = context.Games
                                   .AsNoTracking()
                                   .ToList();

                return games ?? new List<Game>();
            }
        }
    }
