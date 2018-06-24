using GigHub.Core.Models;
using GigHub.Core.Repositories;
using System.Collections.Generic;
using System.Linq;

namespace GigHub.Persistence.Repositories
{
    public class UserNotificationEfRepository : UserNotificationRepository
    {
        private readonly ApplicationDbContext context;

        public UserNotificationEfRepository(ApplicationDbContext context)
        {
            this.context = context;
        }

        public IEnumerable<UserNotification> GetUnreadUserNotifications(string userId)
        {
            return context.UserNotifications
                .Where(un => un.UserId == userId && !un.IsRead)
                .ToList();
        }
    }
}