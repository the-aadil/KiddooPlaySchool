using MediatR;

namespace School.Application.Features.ClassRooms.Commands;

public class AssignTeacherCommand : IRequest<Unit>
{
    public Guid TeacherProfileId { get; set; }
    public Guid ClassRoomId { get; set; }
}
