using MediatR;
using School.Application.DTOs.ClassRooms;

namespace School.Application.Features.ClassRooms.Queries;

public class GetClassRoomByIdQuery : IRequest<ClassRoomResponse>
{
    public Guid Id { get; set; }
}
