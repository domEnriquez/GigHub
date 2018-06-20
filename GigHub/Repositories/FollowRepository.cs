using GigHub.Models;
using System.Linq;

namespace GigHub.Repositories
{
    public class FollowRepository
    {
        private readonly ApplicationDbContext context;

        public FollowRepository(ApplicationDbContext context)
        {
            this.context = context;
        }

        public Follow GetFollowing(string followeeId, string followerId)
        {
            return context
                .Follow
                .SingleOrDefault(f => f.FolloweeId == followeeId && f.FollowerId == followerId);
        }
    }
}