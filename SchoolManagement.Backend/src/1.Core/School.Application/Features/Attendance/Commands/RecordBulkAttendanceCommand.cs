using MediatR;
using School.Application.DTOs.Attendance;
using School.Domain.Enums;

namespace School.Application.Features.Attendance.Commands;

public class RecordBulkAttendanceCommand : IRequest<List<AttendanceRecordResponse>>
{
    public Guid ClassRoomId { get; set; }
    public DateOnly Date { get; set; }
    public List<BulkAttendanceItem> Students { get; set; } = [];
}

public class BulkAttendanceItem
{
    public Guid StudentId { get; set; }
    public TimeOnly? CheckInTime { get; set; }
    public TimeOnly? CheckOutTime { get; set; }
    public AttendanceStatus Status { get; set; }
    public string? Notes { get; set; }
}
