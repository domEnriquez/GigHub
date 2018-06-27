using FluentAssertions;
using GigHub.Core.Models;
using GigHub.Persistence;
using GigHub.Persistence.Repositories;
using GigHub.Tests.Extensions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Data.Entity;

namespace GigHub.Tests.Persistence.Repositories
{

    [TestClass]
    public class GigRepositoryTests
    {
        private GigEfRepository repository;
        Mock<DbSet<Gig>> mockGigs;

        [TestInitialize]
        public void TestInitialize()
        {
            mockGigs = new Mock<DbSet<Gig>>();

            var mockContext = new Mock<IApplicationDbContext>();
            mockContext.SetupGet(c => c.Gigs).Returns(mockGigs.Object);

            repository = new GigEfRepository(mockContext.Object);
        }

        [TestMethod]
        public void GetUpcomingGigsByArtist_GigIsInThePast_ShouldNotBeReturned()
        {
            var gig = new Gig {
                DateTime = DateTime.Now.AddDays(-1) ,
                ArtistId = "1"
            };

            mockGigs.SetSource(new[] { gig });

            IEnumerable<Gig> gigs = repository.GetUpcomingGigsByArtist("1");

            gigs.Should().BeEmpty();
        }

        [TestMethod]
        public void GetUpcomingGigsByArtist_GigIsCancelled_ShouldNotBeReturned()
        {
            var gig = new Gig
            {
                DateTime = DateTime.Now.AddDays(1),
                ArtistId = "1"
            };
            gig.Cancel();

            mockGigs.SetSource(new[] { gig });

            IEnumerable<Gig> gigs = repository.GetUpcomingGigsByArtist("1");

            gigs.Should().BeEmpty();
        }

        [TestMethod]
        public void GetUpcomingGigsByArtist_GigIsForDifferentArtist_ShouldNotBeReturned()
        {
            var gig = new Gig
            {
                DateTime = DateTime.Now.AddDays(1),
                ArtistId = "1"
            };

            mockGigs.SetSource(new[] { gig });

            IEnumerable<Gig> gigs = repository.GetUpcomingGigsByArtist(gig.ArtistId + "-");

            gigs.Should().BeEmpty();
        }

        [TestMethod]
        public void GetUpcomingGigsByArtist_GigsIsForTheGivenArtistAndIsInTheFuture_ShouldBeReturned()
        {
            var gig = new Gig
            {
                DateTime = DateTime.Now.AddDays(1),
                ArtistId = "1"
            };

            mockGigs.SetSource(new[] { gig });

            IEnumerable<Gig> gigs = repository.GetUpcomingGigsByArtist(gig.ArtistId);

            gigs.Should().Contain(gig);
        }
    }
}
