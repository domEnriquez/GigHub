using GigHub.Core.Models;
using GigHub.Core.Repositories;
using System.Collections.Generic;
using System.Linq;

namespace GigHub.Persistence.Repositories
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