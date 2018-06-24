using GigHub.Core;
using GigHub.Core.Models;
using Microsoft.AspNet.Identity;
using System.Web.Http;

namespace GigHub.Controllers.Api
{
    [Authorize]
    public class GigsController : ApiController
    {
        private readonly UnitOfWork unitOfWork;

        public GigsController(UnitOfWork uow)
        {
            unitOfWork = uow;
        }

        [HttpDelete]
        public IHttpActionResult Cancel(int id)
        {
            Gig gig = unitOfWork.Gigs.GetGigWithAttendees(id);

            if (gig.ArtistId != User.Identity.GetUserId())
                return Unauthorized();

            if (gig.IsCancelled)
                return NotFound();

            gig.Cancel();
            unitOfWork.Complete();

            return Ok();
        }
    }
}
