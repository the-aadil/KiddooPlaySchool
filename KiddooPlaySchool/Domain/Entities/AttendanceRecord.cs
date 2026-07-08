using KiddooPlaySchool.Domain.Enums;

namespace KiddooPlaySchool.Domain.Entities;

public class AttendanceRecord : BaseEntity
{
    public Guid StudentId { get; set; }
    public DateOnly Date { get; set; }
    public TimeOnly? CheckInTime { get; set; }
    public TimeOnly? CheckOutTime { get; set; }
    public AttendanceStatus Status { get; set; }
    public string? Notes { get; set; }
    public Guid RecordedById { get; set; }

    public StudentProfile Student { get; set; } = null!;
    public ApplicationUser RecordedBy { get; set; } = null!;
}
