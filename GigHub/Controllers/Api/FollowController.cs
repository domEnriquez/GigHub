using GigHub.Core;
using GigHub.Core.Dtos;
using GigHub.Core.Models;
using Microsoft.AspNet.Identity;
using System.Web.Http;

namespace GigHub.Controllers.Api
{
    public class FollowController : ApiController
    {
        private readonly UnitOfWork unitOfWork;

        public FollowController(UnitOfWork uow)
        {
            unitOfWork = uow;
        }

        [HttpPost]
        public IHttpActionResult Follow(FollowDto dto)
        {
            if (unitOfWork.Follows.GetFollowing(dto.FolloweeId, User.Identity.GetUserId()) != null)
                return BadRequest("Following already exists");

            var follow = new Follow
            {
                FolloweeId = dto.FolloweeId,
                FollowerId = User.Identity.GetUserId()
            };

            unitOfWork.Follows.Add(follow);
            unitOfWork.Complete();

            return Ok();
        }

        [HttpDelete]
        public IHttpActionResult UnFollow(string id)
        {
            Follow follow = unitOfWork.Follows.GetFollowing(id, User.Identity.GetUserId());

            if (follow == null)
                return NotFound();

            unitOfWork.Follows.Remove(follow);
            unitOfWork.Complete();

            return Ok(id);
        }
    }
}
