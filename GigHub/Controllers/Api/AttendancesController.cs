using GigHub.Dtos;
using GigHub.Models;
using Microsoft.AspNet.Identity;
using System.Linq;
using System.Web.Http;

namespace GigHub.Controllers.Api
{
    [Authorize]
    public class AttendancesController : ApiController
    {
        private ApplicationDbContext context;

        public AttendancesController()
        {
            context = new ApplicationDbContext();
        }

        [HttpPost]
        public IHttpActionResult Attend(AttendanceDto dto)
        {
            string userId = User.Identity.GetUserId();

            if (context.Attendances.Any(a => a.AttendeeId == userId && a.GigId == dto.GigId))
                return BadRequest("The attendance already exists");

            var attendance = new Attendance
            {
                GigId = dto.GigId,
                AttendeeId = User.Identity.GetUserId()
            };

            context.Attendances.Add(attendance);
            context.SaveChanges();

            return Ok();
        }

        [HttpDelete]
        public IHttpActionResult DeleteAttendance(int id)
        {
            string userId = User.Identity.GetUserId();

            Attendance attendance = context
                .Attendances
                .SingleOrDefault(a => a.AttendeeId == userId && a.GigId == id);

            if (attendance == null)
                return NotFound();

            context.Attendances.Remove(attendance);
            context.SaveChanges();

            return Ok(id);
        }
    }
}
