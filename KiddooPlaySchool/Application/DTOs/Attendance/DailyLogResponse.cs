using KiddooPlaySchool.Domain.Enums;

namespace KiddooPlaySchool.Application.DTOs.Attendance;

public class DailyLogResponse
{
    public Guid Id { get; set; }
    public Guid StudentId { get; set; }
    public string StudentName { get; set; } = string.Empty;
    public string TeacherName { get; set; } = string.Empty;
    public DateTimeOffset LogTimestamp { get; set; }
    public ActivityType ActivityType { get; set; }
    public string ActivityTypeName { get; set; } = string.Empty;
    public string? Payload { get; set; }
    public ActivityVisibility Visibility { get; set; }
    public string? Remarks { get; set; }
    public string? MediaUrls { get; set; }
    public DateTime CreatedAt { get; set; }
}
