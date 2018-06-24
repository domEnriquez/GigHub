using GigHub.Core.Models;
using GigHub.Core.Repositories;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

namespace GigHub.Persistence.Repositories
{
    public class GigEfRepository : GigRepository
    {
        private readonly ApplicationDbContext context;

        public GigEfRepository(ApplicationDbContext context)
        {
            this.context = context;
        }

        public Gig GetGig(int gigId)
        {
            return context.Gigs
                .Include(g => g.Artist)
                .Include(g => g.Genre)
                .SingleOrDefault(g => g.Id == gigId);
        }

        public IEnumerable<Gig> GetUpcomingGigsByArtist(string artistId)
        {
            return context.Gigs
                .Where(g =>
                    g.ArtistId == artistId &&
                    g.DateTime > DateTime.Now &&
                    !g.IsCancelled)
                .Include(g => g.Genre)
                .ToList();
        }

        public Gig GetGigWithAttendees(int gigId)
        {
            return context.Gigs
                .Include(g => g.Attendances.Select(a => a.Attendee))
                .SingleOrDefault(g => g.Id == gigId);
        }

        public IEnumerable<Gig> GetGigsUserAttending(string userId)
        {
            return context.Attendances
                .Where(a => a.AttendeeId == userId)
                .Select(a => a.Gig)
                .Include(g => g.Artist)
                .Include(g => g.Genre)
                .ToList();
        }

        public void AddGig(Gig gig)
        {
            context.Gigs.Add(gig);
        }

        public IEnumerable<Gig> GetAllUpcomingGigs()
        {
            return context.Gigs
                .Include(g => g.Artist)
                .Include(g => g.Genre)
                .Where(g => g.DateTime > DateTime.Now && !g.IsCancelled);
        }
    }
}