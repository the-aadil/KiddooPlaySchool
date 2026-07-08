using School.Domain.Enums;

namespace School.Application.DTOs.DailyLogs;

public class DailyLogResponse
{
    public Guid Id { get; set; }
    public Guid StudentId { get; set; }
    public string StudentName { get; set; } = string.Empty;
    public string TeacherName { get; set; } = string.Empty;
    public DateTimeOffset LogTimestamp { get; set; }
    public string ActivityType { get; set; } = string.Empty;
    public string? Payload { get; set; }
    public string Visibility { get; set; } = string.Empty;
    public string? Remarks { get; set; }
    public List<string> MediaUrls { get; set; } = [];
    public DateTimeOffset CreatedAt { get; set; }
}
