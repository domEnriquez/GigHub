using GigHub.Core.Models;
using GigHub.Core.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;

namespace GigHub.Persistence.Repositories
{
    public class AttendanceEfRepository : AttendanceRepository
    {
        private readonly ApplicationDbContext context;

        public AttendanceEfRepository(ApplicationDbContext context)
        {
            this.context = context;
        }


        public Attendance GetAttendance(int gigId, string userId)
        {
            return context
                .Attendances
                .SingleOrDefault(a => a.GigId == gigId && a.AttendeeId == userId);
        }

        public IEnumerable<Attendance> GetFutureAttendances(string userId)
        {
            return context.Attendances
                            .Where(a => a.AttendeeId == userId && a.Gig.DateTime > DateTime.Now)
                            .ToList();
        }

        public void Add(Attendance attendance)
        {
            context.Attendances.Add(attendance);
        }

        public void Remove(Attendance attendance)
        {
            context.Attendances.Remove(attendance);
        }
    }
}