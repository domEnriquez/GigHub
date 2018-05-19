using GigHub.Models;
using Microsoft.AspNet.Identity;
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

            context.SaveChanges();

            return Ok();
        }

    }
}
