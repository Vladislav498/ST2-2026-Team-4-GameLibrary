using Microsoft.EntityFrameworkCore.Metadata.Conventions;
using ST2_2026_Team_4_GameLibrary.Models;


namespace ST2_2026_Team_4_GameLibrary.Services
{
    //suzdavam kolekciq s id-ta na igri, posle shte polzvame kato foreach za da se pokajat v nqkoe ot viewtata

    public class FeaturedGamesSingleton
    {

        private readonly HashSet<int> featuredGameIds = new();

        public IReadOnlyCollection<int> FeaturedGameIds =>
            featuredGameIds.ToArray();

        public bool IsFeatured(int gameId)
        {
            return featuredGameIds.Contains(gameId);
        }

        public void Add(int gameId)
        {
            featuredGameIds.Add(gameId);
        }

        public void Remove(int gameId)
        {
            featuredGameIds.Remove(gameId);
        }

      
    }
}
