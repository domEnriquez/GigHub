using GigHub.Models;

namespace GigHub.Repositories
{
    public interface FollowRepository
    {
        Follow GetFollowing(string followeeId, string followerId);
    }
}