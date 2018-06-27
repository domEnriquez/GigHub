using FluentAssertions;
using GigHub.Controllers.Api;
using GigHub.Core;
using GigHub.Core.Models;
using GigHub.Core.Repositories;
using GigHub.Tests.Extensions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System.Web.Http.Results;

namespace GigHub.Tests.Controllers.Api
{
    [TestClass]
    public class GigsControllerTests
    {
        private GigsController controller;
        Mock<GigRepository> mockRepository;
        private string userId = "1";

        [TestInitialize]
        public void TestInitialize()
        {
            mockRepository = new Mock<GigRepository>();

            var mockUow = new Mock<UnitOfWork>();
            mockUow.SetupGet(u => u.Gigs).Returns(mockRepository.Object);

            controller = new GigsController(mockUow.Object);
            controller.MockCurrentUser(userId, "user1@domain.com");
        }

        [TestMethod]
        public void Cancel_NoGigWithGivenIdExists_ShouldReturnNotFound()
        {
            var result = controller.Cancel(1);

            result.Should().BeOfType<NotFoundResult>();
        }

        [TestMethod]
        public void Cancel_GigIsCancelled_ShouldReturnNotFound()
        {
            var gig = new Gig();
            gig.Cancel();

            mockRepository.Setup(r => r.GetGigWithAttendees(1)).Returns(gig);

            var result = controller.Cancel(1);

            result.Should().BeOfType<NotFoundResult>();
        }

        [TestMethod]
        public void Cancel_UserCancelingAnotherUsersGig_ShouldReturnUnauthorized()
        {
            var gig = new Gig { ArtistId = userId + "-" };

            mockRepository.Setup(r => r.GetGigWithAttendees(1)).Returns(gig);

            var result = controller.Cancel(1);

            result.Should().BeOfType<UnauthorizedResult>();
        }

        [TestMethod]
        public void Cancel_ValidRequest_ShouldReturnOk()
        {
            var gig = new Gig { ArtistId = userId };

            mockRepository.Setup(r => r.GetGigWithAttendees(1)).Returns(gig);

            var result = controller.Cancel(1);

            result.Should().BeOfType<OkResult>();
        }
    }
}
