using MediatR;
using School.Application.DTOs.ClassRooms;
using School.Application.DTOs.Common;

namespace School.Application.Features.ClassRooms.Queries;

public class GetClassRoomsQuery : IRequest<PagedResponse<ClassRoomResponse>>
{
    public int PageNumber { get; set; } = 1;
    public int PageSize { get; set; } = 20;
    public bool? IsActive { get; set; }
}
