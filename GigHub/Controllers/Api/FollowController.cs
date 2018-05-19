using GigHub.Models;
using Microsoft.AspNet.Identity;
using System.Linq;
using System.Web.Http;

namespace GigHub.Controllers.Api
{
    public class FollowController : ApiController
    {
        private ApplicationDbContext context;

        public FollowController()
        {
            context = new ApplicationDbContext();
        }

        [HttpPost]
        public IHttpActionResult Follow(FollowDto dto)
        {
            string userId = User.Identity.GetUserId();

            if (context.Follow.Any(f => f.FolloweeId == dto.FolloweeId && userId == f.FollowerId))
                return BadRequest("Following already exists");

            var follow = new Follow
            {
                FolloweeId = dto.FolloweeId,
                FollowerId = userId
            };

            context.Follow.Add(follow);
            context.SaveChanges();

            return Ok();
        }
    }
}
