using GigHub.Core.Models;

namespace GigHub.Core.Repositories
{
    public interface FollowRepository
    {
        Follow GetFollowing(string followeeId, string followerId);
        void Add(Follow follow);
        void Remove(Follow follow);
    }
}