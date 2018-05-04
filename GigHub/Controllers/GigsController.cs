﻿using GigHub.Models;
using GigHub.ViewModels;
using Microsoft.AspNet.Identity;
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
        public ActionResult Create()
        {
            var viewModel = new GigFormViewModel
            {
                Genres = context.Genres.ToList()
            };

            return View(viewModel);
        }

        [Authorize]
        [HttpPost]
        public ActionResult Create(GigFormViewModel viewModel)
        {
            if(!ModelState.IsValid)
            {
                viewModel.Genres = context.Genres.ToList();

                return View("Create", viewModel);
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

            return RedirectToAction("Index", "Home");
        }
    }
}