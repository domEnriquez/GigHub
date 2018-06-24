﻿using GigHub.Core.Models;
using System.Collections.Generic;

namespace GigHub.Core.Repositories
{
    public interface UserNotificationRepository
    {
        IEnumerable<UserNotification> GetUnreadUserNotifications(string userId);
    }
}