using GigHub.Core.Repositories;

namespace GigHub.Core
{
    public interface UnitOfWork
    {
        AttendanceRepository Attendances { get; }
        FollowRepository Follows { get; }
        GenreRepository Genres { get; }
        GigRepository Gigs { get; }
        NotificationRepository Notifications { get; }
        UserNotificationRepository UserNotifications { get; }
        void Complete();
    }
}