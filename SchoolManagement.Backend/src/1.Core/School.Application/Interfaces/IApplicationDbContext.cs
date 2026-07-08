using Microsoft.EntityFrameworkCore;
using School.Domain.Entities;

namespace School.Application.Interfaces;

public interface IApplicationDbContext
{
    DbSet<ApplicationUser> ApplicationUsers { get; }
    DbSet<AdminProfile> AdminProfiles { get; }
    DbSet<TeacherProfile> TeacherProfiles { get; }
    DbSet<StudentProfile> StudentProfiles { get; }
    DbSet<ClassRoom> ClassRooms { get; }
    DbSet<TeacherClassRoomAssignment> TeacherClassRoomAssignments { get; }
    DbSet<AttendanceRecord> AttendanceRecords { get; }
    DbSet<DailyActivityLog> DailyActivityLogs { get; }

    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}
