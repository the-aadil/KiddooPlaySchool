using School.Domain.Common;

namespace School.Domain.Entities;

public class StudentProfile : BaseAuditableEntity
{
    public Guid UserId { get; set; }
    public DateTime DateOfBirth { get; set; }
    public string? Address { get; set; }
    public string? ParentName { get; set; }
    public string? ParentPhone { get; set; }
    public string? ParentEmail { get; set; }
    public DateTime EnrollmentDate { get; set; }
    public Guid? ClassRoomId { get; set; }

    public ApplicationUser User { get; set; } = null!;
    public ClassRoom? ClassRoom { get; set; }
}
