using GigHub.Core.Models;
using GigHub.Core.Repositories;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

namespace GigHub.Persistence.Repositories
{
    public class NotificationEfRepository : NotificationRepository
    {
        private readonly ApplicationDbContext context;

        public NotificationEfRepository(ApplicationDbContext context)
        {
            this.context = context;
        }

        public IEnumerable<Notification> GetUnreadNotifications(string userId)
        {
            return context.UserNotifications
                .Where(un => un.UserId == userId && !un.IsRead)
                .Select(un => un.Notification)
                .Include(n => n.Gig.Artist)
                .ToList();
        }
    }
}