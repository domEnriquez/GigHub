using System.Collections.Generic;
using GigHub.Models;

namespace GigHub.Repositories
{
    public interface AttendanceRepository
    {
        Attendance GetAttendance(int gigId, string userId);
        IEnumerable<Attendance> GetFutureAttendances(string userId);
    }
}