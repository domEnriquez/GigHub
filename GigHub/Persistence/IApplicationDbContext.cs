using System.Data.Entity;
using GigHub.Core.Models;

namespace GigHub.Persistence
{
    public interface IApplicationDbContext
    {
        DbSet<Attendance> Attendances { get; set; }
        DbSet<Follow> Follow { get; set; }
        DbSet<Genre> Genres { get; set; }
        DbSet<Gig> Gigs { get; set; }
        DbSet<Notification> Notifications { get; set; }
        DbSet<UserNotification> UserNotifications { get; set; }
    }
}