using GigHub.Models;
using GigHub.Repositories;

namespace GigHub.Persistence
{
    public class UnitOfWorkEf : UnitOfWork
    {
        private readonly ApplicationDbContext context;

        public GigRepository Gigs { get; private set; }
        public AttendanceRepository Attendances { get; private set; }
        public FollowRepository Follows { get; private set; }
        public GenreRepository Genres { get; private set; }


        public UnitOfWorkEf(ApplicationDbContext context)
        {
            this.context = context;
            Gigs = new GigEfRepository(context);
            Attendances = new AttendanceEfRepository(context);
            Follows = new FollowEfRepository(context);
            Genres = new GenreEfRepository(context);
        }

        public void Complete()
        {
            context.SaveChanges();
        }
    }
}