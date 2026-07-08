using School.Domain.Enums;

namespace School.Application.DTOs.Attendance;

public class RecordBulkAttendanceRequest
{
    public Guid ClassRoomId { get; set; }
    public DateOnly Date { get; set; }
    public List<StudentAttendanceItem> Students { get; set; } = [];
}

public class StudentAttendanceItem
{
    public Guid StudentId { get; set; }
    public TimeOnly? CheckInTime { get; set; }
    public TimeOnly? CheckOutTime { get; set; }
    public AttendanceStatus Status { get; set; }
    public string? Notes { get; set; }
}
