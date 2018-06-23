using GigHub.Models;
using System.Collections.Generic;
using System.Linq;

namespace GigHub.Repositories
{
    public class GenreEfRepository : GenreRepository
    {
        private readonly ApplicationDbContext context;

        public GenreEfRepository(ApplicationDbContext context)
        {
            this.context = context;
        }

        public IEnumerable<Genre> GetGenres()
        {
            return context.Genres.ToList();
        }
    }
}