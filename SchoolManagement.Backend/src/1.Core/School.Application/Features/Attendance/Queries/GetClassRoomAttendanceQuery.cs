using MediatR;
using School.Application.DTOs.Attendance;

namespace School.Application.Features.Attendance.Queries;

public class GetClassRoomAttendanceQuery : IRequest<List<AttendanceRecordResponse>>
{
    public Guid ClassRoomId { get; set; }
    public DateOnly Date { get; set; }
}
