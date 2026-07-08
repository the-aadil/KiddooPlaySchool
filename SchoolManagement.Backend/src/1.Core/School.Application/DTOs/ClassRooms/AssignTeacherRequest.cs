namespace School.Application.DTOs.ClassRooms;

public class AssignTeacherRequest
{
    public Guid TeacherProfileId { get; set; }
    public Guid ClassRoomId { get; set; }
}
