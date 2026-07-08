namespace KiddooPlaySchool.Application.DTOs.Attendance;

public class StudentDailyFeedResponse
{
    public DateOnly Date { get; set; }
    public AttendanceSummary? Attendance { get; set; }
    public List<DailyLogResponse> Activities { get; set; } = [];
}

public class AttendanceSummary
{
    public Guid AttendanceId { get; set; }
    public string Status { get; set; } = string.Empty;
    public TimeOnly? CheckInTime { get; set; }
    public TimeOnly? CheckOutTime { get; set; }
    public string? Notes { get; set; }
}
