using School.Domain.Common;
using School.Domain.Enums;

namespace School.Domain.Entities;

public class ClassRoom : BaseAuditableEntity
{
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    public int Capacity { get; set; }
    public AgeGroup AgeGroup { get; set; }
    public bool IsActive { get; set; } = true;

    public ICollection<StudentProfile> Students { get; set; } = [];
    public ICollection<TeacherClassRoomAssignment> TeacherClassRoomAssignments { get; set; } = [];
}
