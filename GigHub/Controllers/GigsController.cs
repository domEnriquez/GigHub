using GigHub.Models;
using GigHub.Persistence;
using GigHub.ViewModels;
using Microsoft.AspNet.Identity;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace GigHub.Controllers
{
    public class GigsController : Controller
    {
        private readonly UnitOfWork unitOfWork;

        public GigsController()
        {
            unitOfWork = new UnitOfWorkEf(new ApplicationDbContext());
        }

        [Authorize]
        public ActionResult Mine()
        {
            string userId = User.Identity.GetUserId();
            IEnumerable<Gig> gigs = unitOfWork.Gigs.GetUpcomingGigsByArtist(userId);

            return View(gigs);
        }

        [Authorize]
        public ActionResult Attending()
        {
            string userId = User.Identity.GetUserId();

            var viewModel = new GigsViewModel
            {
                UpcomingGigs = unitOfWork.Gigs.GetGigsUserAttending(userId),
                ShowActions = User.Identity.IsAuthenticated,
                Heading = "Gigs I'm Attending",
                Attendances = unitOfWork.Attendances.GetFutureAttendances(userId).ToLookup(a => a.GigId)
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
                Genres = unitOfWork.Genres.GetGenres(),
                Heading = "Add a Gig"
            };

            return View("GigForm", viewModel);
        }

        [Authorize]
        public ActionResult Edit(int id)
        {
            string userId = User.Identity.GetUserId();
            Gig gig = unitOfWork.Gigs.GetGig(id);

            if (gig == null)
                return HttpNotFound();

            if (gig.ArtistId != userId)
                return new HttpUnauthorizedResult();

            var viewModel = new GigFormViewModel
            {
                Heading = "Edit a Gig",
                Id = gig.Id,
                Genres = unitOfWork.Genres.GetGenres(),
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
                viewModel.Genres = unitOfWork.Genres.GetGenres();

                return View("GigForm", viewModel);
            }

            Gig gig = new Gig
            {
                ArtistId = User.Identity.GetUserId(),
                DateTime = viewModel.GetDateTime(),
                GenreId = viewModel.Genre,
                Venue = viewModel.Venue
            };

            unitOfWork.Gigs.AddGig(gig);
            unitOfWork.Complete();

            return RedirectToAction("Mine", "Gigs");
        }

        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Update(GigFormViewModel viewModel)
        {
            if (!ModelState.IsValid)
            {
                viewModel.Genres = unitOfWork.Genres.GetGenres();

                return View("GigForm", viewModel);
            }

            Gig gig = unitOfWork.Gigs.GetGigWithAttendees(viewModel.Id);

            if (gig == null)
                return HttpNotFound();

            if (gig.ArtistId != User.Identity.GetUserId())
                return new HttpUnauthorizedResult();

            gig.Modify(viewModel.Venue, viewModel.GetDateTime(), viewModel.Genre);

            unitOfWork.Complete();

            return RedirectToAction("Mine", "Gigs");
        }

        [HttpGet]
        public ActionResult Details(int id)
        {
            Gig gig = unitOfWork.Gigs.GetGig(id);

            if (gig == null)
                return HttpNotFound();

            GigDetailsViewModel viewModel = new GigDetailsViewModel { Gig = gig };

            if(User.Identity.IsAuthenticated)
            {
                string userId = User.Identity.GetUserId();

                viewModel.IsAttending = 
                    unitOfWork.Attendances.GetAttendance(gig.Id, userId) != null;

                viewModel.IsFollowing = 
                    unitOfWork.Follows.GetFollowing(gig.ArtistId, userId) != null;
            }

            return View(viewModel);
        }
    }
}