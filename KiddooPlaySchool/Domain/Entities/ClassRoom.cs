using KiddooPlaySchool.Domain.Enums;

namespace KiddooPlaySchool.Domain.Entities;

public class ClassRoom : BaseEntity
{
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    public int Capacity { get; set; }
    public AgeGroup AgeGroup { get; set; }
    public bool IsActive { get; set; } = true;

    public ICollection<StudentProfile> Students { get; set; } = new List<StudentProfile>();
    public ICollection<TeacherClassRoom> TeacherClassRooms { get; set; } = new List<TeacherClassRoom>();
}
