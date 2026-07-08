namespace KiddooPlaySchool.Domain.Entities;

public class TeacherClassRoom
{
    public Guid TeacherProfileId { get; set; }
    public Guid ClassRoomId { get; set; }
    public DateTime AssignmentDate { get; set; }

    public TeacherProfile TeacherProfile { get; set; } = null!;
    public ClassRoom ClassRoom { get; set; } = null!;
}
