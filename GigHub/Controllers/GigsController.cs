using GigHub.Models;
using GigHub.ViewModels;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web.Mvc;

namespace GigHub.Controllers
{
    public class GigsController : Controller
    {
        private ApplicationDbContext context;

        public GigsController()
        {
            context = new ApplicationDbContext();
        }

        [Authorize]
        public ActionResult Mine()
        {
            string userId = User.Identity.GetUserId();
            List<Gig> gigs = context.Gigs
                .Where(g => 
                    g.ArtistId == userId && 
                    g.DateTime > DateTime.Now && 
                    !g.IsCancelled)
                .Include(g => g.Genre)
                .ToList();

            return View(gigs);
        }

        [Authorize]
        public ActionResult Attending()
        {
            string userId = User.Identity.GetUserId();
            List<Gig> gigs = context.Attendances
                .Where(a => a.AttendeeId == userId)
                .Select(a => a.Gig)
                .Include(g => g.Artist)
                .Include(g => g.Genre)
                .ToList();

            var viewModel = new GigsViewModel
            {
                UpcomingGigs = gigs,
                ShowActions = User.Identity.IsAuthenticated,
                Heading = "Gigs I'm Attending"
            };

            return View("Gigs", viewModel);
        }

        [HttpPost]
        public ActionResult Search(GigsViewModel viewModel)
        {
            return RedirectToAction("Index", "Home", new { query = viewModel.SearchTerm });
        }

        [Authorize]
        public ActionResult Create()
        {
            var viewModel = new GigFormViewModel
            {
                Genres = context.Genres.ToList(),
                Heading = "Add a Gig"
            };

            return View("GigForm", viewModel);
        }

        [Authorize]
        public ActionResult Edit(int id)
        {
            string userId = User.Identity.GetUserId();
            Gig gig = context.Gigs.Single(g => g.Id == id && g.ArtistId == userId);

            var viewModel = new GigFormViewModel
            {
                Heading = "Edit a Gig",
                Id = gig.Id,
                Genres = context.Genres.ToList(),
                Date = gig.DateTime.ToString("d MMM yyyy"),
                Time = gig.DateTime.ToString("HH:mm"),
                Genre = gig.GenreId,
                Venue = gig.Venue
            };

            return View("GigForm", viewModel);
        }

        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(GigFormViewModel viewModel)
        {
            if(!ModelState.IsValid)
            {
                viewModel.Genres = context.Genres.ToList();

                return View("GigForm", viewModel);
            }

            Gig gig = new Gig
            {
                ArtistId = User.Identity.GetUserId(),
                DateTime = viewModel.GetDateTime(),
                GenreId = viewModel.Genre,
                Venue = viewModel.Venue
            };

            context.Gigs.Add(gig);
            context.SaveChanges();

            return RedirectToAction("Mine", "Gigs");
        }

        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Update(GigFormViewModel viewModel)
        {
            if (!ModelState.IsValid)
            {
                viewModel.Genres = context.Genres.ToList();

                return View("GigForm", viewModel);
            }

            string userId = User.Identity.GetUserId();
            Gig gig = context.Gigs
                .Include(g => g.Attendances.Select(a => a.Attendee))
                .Single(g => g.Id == viewModel.Id && g.ArtistId == userId);

            gig.Modify(viewModel.Venue, viewModel.GetDateTime(), viewModel.Genre);

            context.SaveChanges();

            return RedirectToAction("Mine", "Gigs");
        }

        [HttpGet]
        public ActionResult Details(int id)
        {
            bool loggedIn = User.Identity.IsAuthenticated;
            Gig gig = context.Gigs
                .Include(g => g.Artist)
                .Include(g => g.Genre)
                .Single(g => g.Id == id);

            if (gig == null)
                return HttpNotFound();

            GigDetailsViewModel viewModel = new GigDetailsViewModel { Gig = gig };

            if(User.Identity.IsAuthenticated)
            {
                string userId = User.Identity.GetUserId();

                viewModel.IsAttending = context.Attendances
                    .Any(a => a.GigId == gig.Id && a.AttendeeId == userId);

                viewModel.IsFollowing = context.Follow
                    .Any(f => f.FolloweeId == gig.ArtistId && f.FollowerId == userId);
            }

            return View(viewModel);
        }
    }
}