using GigHub.Repositories;

namespace GigHub.Persistence
{
    public interface UnitOfWork
    {
        AttendanceRepository Attendances { get; }
        FollowRepository Follows { get; }
        GenreRepository Genres { get; }
        GigRepository Gigs { get; }

        void Complete();
    }
}