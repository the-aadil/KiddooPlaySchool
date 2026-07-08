namespace KiddooPlaySchool.Application.DTOs.ClassRoom;

public class AssignTeacherRequest
{
    public Guid TeacherProfileId { get; set; }
    public Guid ClassRoomId { get; set; }
}
