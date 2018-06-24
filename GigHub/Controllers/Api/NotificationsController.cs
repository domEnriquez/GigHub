using AutoMapper;
using GigHub.Core;
using GigHub.Core.Dtos;
using GigHub.Core.Models;
using Microsoft.AspNet.Identity;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;

namespace GigHub.Controllers.Api
{
    [Authorize]
    public class NotificationsController : ApiController
    {
        private readonly UnitOfWork unitOfWork;

        public NotificationsController(UnitOfWork uow)
        {
            unitOfWork = uow;
        }

        public IEnumerable<NotificationDto> GetNewNotifications()
        {
            List<Notification> notifications = unitOfWork
                .Notifications
                .GetUnreadNotifications(User.Identity.GetUserId())
                .ToList();

            return notifications.Select(Mapper.Map<Notification, NotificationDto>);
        }

        [HttpPost]
        public IHttpActionResult MarkAsRead()
        {
            string userId = User.Identity.GetUserId();

            List<UserNotification> userNotifs = unitOfWork.UserNotifications.GetUnreadUserNotifications(userId).ToList();

            userNotifs.ForEach(un => un.Read());

            unitOfWork.Complete();

            return Ok();
        }
    }
}
