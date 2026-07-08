using KiddooPlaySchool.Application.DTOs.Attendance;

namespace KiddooPlaySchool.Application.Interfaces;

public interface IDailyLogService
{
    Task<DailyLogResponse> CreateAsync(CreateDailyLogRequest request, Guid teacherProfileId);
    Task<IEnumerable<DailyLogResponse>> GetStudentFeedAsync(Guid studentId, Guid? currentUserId, string currentUserRole);
    Task<IEnumerable<DailyLogResponse>> GetStudentLogsByDateAsync(Guid studentId, DateOnly date, Guid? currentUserId, string currentUserRole);
}
