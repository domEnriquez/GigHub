using System.Collections.Generic;
using GigHub.Models;

namespace GigHub.Repositories
{
    public interface GigRepository
    {
        void AddGig(Gig gig);
        Gig GetGig(int gigId);
        IEnumerable<Gig> GetGigsUserAttending(string userId);
        Gig GetGigWithAttendees(int gigId);
        IEnumerable<Gig> GetUpcomingGigsByArtist(string artistId);
    }
}