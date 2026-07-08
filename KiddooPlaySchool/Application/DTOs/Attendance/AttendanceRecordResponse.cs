using KiddooPlaySchool.Domain.Enums;

namespace KiddooPlaySchool.Application.DTOs.Attendance;

public class AttendanceRecordResponse
{
    public Guid Id { get; set; }
    public Guid StudentId { get; set; }
    public string StudentName { get; set; } = string.Empty;
    public DateOnly Date { get; set; }
    public TimeOnly? CheckInTime { get; set; }
    public TimeOnly? CheckOutTime { get; set; }
    public AttendanceStatus Status { get; set; }
    public string? Notes { get; set; }
    public DateTime CreatedAt { get; set; }
}
