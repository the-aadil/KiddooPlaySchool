using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using School.Application.DTOs.Attendance;
using School.Application.Interfaces;

namespace School.Application.Features.Attendance.Queries;

public class GetClassRoomAttendanceQueryHandler
    : IRequestHandler<GetClassRoomAttendanceQuery, List<AttendanceRecordResponse>>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public GetClassRoomAttendanceQueryHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<List<AttendanceRecordResponse>> Handle(
        GetClassRoomAttendanceQuery request, CancellationToken cancellationToken)
    {
        var records = await _context.AttendanceRecords
            .AsNoTracking()
            .Include(a => a.Student).ThenInclude(s => s.User)
            .Where(a => a.Student.ClassRoomId == request.ClassRoomId
                     && a.Date == request.Date
                     && !a.IsDeleted)
            .OrderBy(a => a.Student.User.FirstName)
            .ToListAsync(cancellationToken);

        return _mapper.Map<List<AttendanceRecordResponse>>(records);
    }
}
