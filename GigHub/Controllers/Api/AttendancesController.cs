using GigHub.Core;
using GigHub.Core.Dtos;
using GigHub.Core.Models;
using Microsoft.AspNet.Identity;
using System.Web.Http;

namespace GigHub.Controllers.Api
{
    [Authorize]
    public class AttendancesController : ApiController
    {
        private readonly UnitOfWork unitOfWork;

        public AttendancesController(UnitOfWork uow)
        {
            unitOfWork = uow;
        }

        [HttpPost]
        public IHttpActionResult Attend(AttendanceDto dto)
        {

            if (unitOfWork.Attendances.GetAttendance(dto.GigId, User.Identity.GetUserId()) != null)
                return BadRequest("The attendance already exists");

            var attendance = new Attendance
            {
                GigId = dto.GigId,
                AttendeeId = User.Identity.GetUserId()
            };

            unitOfWork.Attendances.Add(attendance);
            unitOfWork.Complete();
          
            return Ok();
        }

        [HttpDelete]
        public IHttpActionResult DeleteAttendance(int id)
        {
            Attendance attendance = unitOfWork.Attendances.GetAttendance(id, User.Identity.GetUserId());

            if (attendance == null)
                return NotFound();

            unitOfWork.Attendances.Remove(attendance);
            unitOfWork.Complete();

            return Ok(id);
        }
    }
}
