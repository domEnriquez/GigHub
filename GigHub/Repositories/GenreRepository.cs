using GigHub.Models;
using System.Collections.Generic;

namespace GigHub.Repositories
{
    public interface GenreRepository
    {
        IEnumerable<Genre> GetGenres();
    }
}