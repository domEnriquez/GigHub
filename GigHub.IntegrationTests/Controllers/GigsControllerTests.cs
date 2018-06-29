using FluentAssertions;
using GigHub.Controllers;
using GigHub.Core.Models;
using GigHub.Core.ViewModels;
using GigHub.IntegrationTests.Extensions;
using GigHub.Persistence;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;

namespace GigHub.IntegrationTests.Controllers
{
    [TestFixture]
    public class GigsControllerTests
    {
        private ApplicationDbContext context;
        private GigsController controller;

        [SetUp]
        public void SetUp()
        {
            context = new ApplicationDbContext();
            controller = new GigsController(new UnitOfWorkEf(context));
        }

        [TearDown]
        public void TearDown()
        {
            context.Dispose();
        }

        [Test, Isolated]
        public void Mine_WhenCalled_ShouldReturnUpcomingGigs()
        {
            //Arrange
            ApplicationUser user = context.Users.First();
            controller.MockCurrentUser(user.Id, user.UserName);

            Genre genre = context.Genres.First();
            Gig gig = new Gig {
                Artist = user,
                DateTime = DateTime.Now.AddDays(1),
                Genre = genre,
                Venue = "-" };

            context.Gigs.Add(gig);
            context.SaveChanges();

            //Act
            var result = controller.Mine();

            //Assert
            (result.ViewData.Model as IEnumerable<Gig>).Should().HaveCount(1);
        }

        [Test, Isolated]
        public void Update_WhenCalled_ShouldUpdateTheGivenGig()
        {
            //Arrange
            ApplicationUser user = context.Users.First();
            controller.MockCurrentUser(user.Id, user.UserName);

            Genre genre = context.Genres.Single(g => g.Id == 1);
            Gig gig = new Gig
            {
                Artist = user,
                DateTime = DateTime.Now.AddDays(1),
                Genre = genre,
                Venue = "-"
            };
            context.Gigs.Add(gig);
            context.SaveChanges();

            //Act
            var result = controller.Update(new GigFormViewModel
            {
                Id = gig.Id,
                Date = DateTime.Today.AddMonths(1).ToString("d MMM yyyy"),
                Time = "20:00",
                Venue = "Venue",
                Genre = 2
            });

            //Assert
            context.Entry(gig).Reload();
            gig.DateTime.Should().Be(DateTime.Today.AddMonths(1).AddHours(20));
            gig.Venue.Should().Be("Venue");
            gig.GenreId.Should().Be(2);
        }
    }
}
