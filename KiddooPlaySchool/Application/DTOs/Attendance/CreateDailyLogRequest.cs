using KiddooPlaySchool.Domain.Enums;

namespace KiddooPlaySchool.Application.DTOs.Attendance;

public class CreateDailyLogRequest
{
    public Guid StudentId { get; set; }
    public ActivityType ActivityType { get; set; }
    public DateTimeOffset LogTimestamp { get; set; }
    public string? Payload { get; set; }
    public ActivityVisibility Visibility { get; set; }
    public string? Remarks { get; set; }
    public string? MediaUrls { get; set; }
}
