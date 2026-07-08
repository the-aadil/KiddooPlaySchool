using KiddooPlaySchool.Application.DTOs.Attendance;

namespace KiddooPlaySchool.Application.Interfaces;

public interface IAttendanceService
{
    Task<List<AttendanceRecordResponse>> RecordBulkAttendanceAsync(RecordBulkAttendanceRequest request, Guid recordedById);
    Task<AttendanceRecordResponse?> GetStudentAttendanceAsync(Guid studentId, DateOnly date);
    Task<IEnumerable<AttendanceRecordResponse>> GetClassRoomAttendanceAsync(Guid classRoomId, DateOnly date);
    Task<IEnumerable<AttendanceRecordResponse>> GetStudentAttendanceRangeAsync(Guid studentId, DateOnly from, DateOnly to);
}
