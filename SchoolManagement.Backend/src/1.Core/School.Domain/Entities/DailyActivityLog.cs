using School.Domain.Common;
using School.Domain.Enums;

namespace School.Domain.Entities;

public class DailyActivityLog : BaseAuditableEntity
{
    public Guid StudentId { get; set; }
    public Guid TeacherId { get; set; }
    public DateTimeOffset LogTimestamp { get; set; }
    public ActivityType ActivityType { get; set; }
    public string Payload { get; set; } = string.Empty;
    public ActivityVisibility Visibility { get; set; }
    public string? Remarks { get; set; }
    public List<string> MediaUrls { get; set; } = [];

    public StudentProfile Student { get; set; } = null!;
    public TeacherProfile Teacher { get; set; } = null!;
}
