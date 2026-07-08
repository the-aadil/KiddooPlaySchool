using KiddooPlaySchool.Domain.Enums;

namespace KiddooPlaySchool.Domain.Entities;

public class DailyActivityLog : BaseEntity
{
    public Guid StudentId { get; set; }
    public Guid TeacherId { get; set; }
    public DateTimeOffset LogTimestamp { get; set; }
    public ActivityType ActivityType { get; set; }
    public string Payload { get; set; } = string.Empty;
    public ActivityVisibility Visibility { get; set; }
    public string? Remarks { get; set; }
    public string? MediaUrls { get; set; }

    public StudentProfile Student { get; set; } = null!;
    public TeacherProfile Teacher { get; set; } = null!;
}
