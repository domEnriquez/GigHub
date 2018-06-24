using GigHub.Core.Models;
using GigHub.Core.Repositories;
using System.Linq;

namespace GigHub.Persistence.Repositories
{
    public class FollowEfRepository : FollowRepository
    {
        private readonly ApplicationDbContext context;

        public FollowEfRepository(ApplicationDbContext context)
        {
            this.context = context;
        }

        public void Add(Follow follow)
        {
            context.Follow.Add(follow);
        }

        public Follow GetFollowing(string followeeId, string followerId)
        {
            return context
                .Follow
                .SingleOrDefault(f => f.FolloweeId == followeeId && f.FollowerId == followerId);
        }

        public void Remove(Follow follow)
        {
            context.Follow.Remove(follow);
        }
    }
}