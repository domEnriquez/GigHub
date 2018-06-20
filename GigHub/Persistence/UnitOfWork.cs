using GigHub.Models;
using GigHub.Repositories;

namespace GigHub.Persistence
{
    public class UnitOfWork
    {
        private readonly ApplicationDbContext context;

        public GigRepository Gigs { get; private set; }
        public AttendanceRepository Attendances { get; private set; }
        public FollowRepository Follows { get; private set; }
        public GenreRepository Genres { get; private set; }


        public UnitOfWork(ApplicationDbContext context)
        {
            this.context = context;
            Gigs = new GigRepository(context);
            Attendances = new AttendanceRepository(context);
            Follows = new FollowRepository(context);
            Genres = new GenreRepository(context);
        }

        public void Complete()
        {
            context.SaveChanges();
        }
    }
}