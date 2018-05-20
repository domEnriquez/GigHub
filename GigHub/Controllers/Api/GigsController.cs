using GigHub.Models;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;

namespace GigHub.Controllers.Api
{
    [Authorize]
    public class GigsController : ApiController
    {
        private ApplicationDbContext context;

        public GigsController()
        {
            context = new ApplicationDbContext();
        }

        [HttpDelete]
        public IHttpActionResult Cancel(int id)
        {
            string userId = User.Identity.GetUserId();
            Gig gig = context.Gigs.Single(g => g.Id == id && g.ArtistId == userId);
            gig.IsCancelled = true;

            if (gig.IsCancelled)
                return NotFound();

            var notification = new Notification
            {
                DateTime = DateTime.Now,
                Gig = gig,
                Type = NotificationType.GigCancelled
            };

            List<ApplicationUser> attendees = context.Attendances
                .Where(a => a.GigId == gig.Id)
                .Select(a => a.Attendee)
                .ToList();

            foreach(ApplicationUser attendee in attendees)
            {
                attendee.Notify(notification);
            }

            context.SaveChanges();

            return Ok();
        }

    }
}
