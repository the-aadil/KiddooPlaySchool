using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using School.Application.DTOs.Attendance;
using School.Application.Interfaces;
using School.Domain.Entities;
using School.Domain.Exceptions;

namespace School.Application.Features.Attendance.Commands;

public class RecordBulkAttendanceCommandHandler
    : IRequestHandler<RecordBulkAttendanceCommand, List<AttendanceRecordResponse>>
{
    private readonly IApplicationDbContext _context;
    private readonly ICurrentUserContext _currentUser;
    private readonly IMapper _mapper;

    public RecordBulkAttendanceCommandHandler(
        IApplicationDbContext context,
        ICurrentUserContext currentUser,
        IMapper mapper)
    {
        _context = context;
        _currentUser = currentUser;
        _mapper = mapper;
    }

    public async Task<List<AttendanceRecordResponse>> Handle(
        RecordBulkAttendanceCommand request, CancellationToken cancellationToken)
    {
        if (request.Date > DateOnly.FromDateTime(DateTime.UtcNow))
        {
            throw new DomainException("Attendance cannot be recorded for future dates.");
        }

        var studentIds = request.Students.Select(s => s.StudentId).ToList();

        var students = await _context.StudentProfiles
            .AsNoTracking()
            .Include(s => s.User)
            .Where(s => studentIds.Contains(s.Id) && s.ClassRoomId == request.ClassRoomId && !s.IsDeleted)
            .ToListAsync(cancellationToken);

        var invalidIds = studentIds.Except(students.Select(s => s.Id)).ToList();
        if (invalidIds.Count != 0)
        {
            throw new DomainException(
                $"Students with IDs [{string.Join(", ", invalidIds)}] were not found in the specified classroom.");
        }

        var existingRecords = await _context.AttendanceRecords
            .Where(a => a.Date == request.Date && studentIds.Contains(a.StudentId) && !a.IsDeleted)
            .ToListAsync(cancellationToken);

        foreach (var item in request.Students)
        {
            if (item.CheckInTime.HasValue && item.CheckOutTime.HasValue
                && item.CheckOutTime <= item.CheckInTime)
            {
                throw new DomainException(
                    $"Check-Out time cannot be earlier than Check-In time for student {item.StudentId}.");
            }

            var existing = existingRecords.FirstOrDefault(a => a.StudentId == item.StudentId);

            if (existing is not null)
            {
                existing.CheckInTime = item.CheckInTime;
                existing.CheckOutTime = item.CheckOutTime;
                existing.Status = item.Status;
                existing.Notes = item.Notes;
                existing.RecordedById = _currentUser.UserId;
            }
            else
            {
                _context.AttendanceRecords.Add(new AttendanceRecord
                {
                    StudentId = item.StudentId,
                    Date = request.Date,
                    CheckInTime = item.CheckInTime,
                    CheckOutTime = item.CheckOutTime,
                    Status = item.Status,
                    Notes = item.Notes,
                    RecordedById = _currentUser.UserId
                });
            }
        }

        await _context.SaveChangesAsync(cancellationToken);

        var allRecords = await _context.AttendanceRecords
            .AsNoTracking()
            .Include(a => a.Student).ThenInclude(s => s.User)
            .Where(a => a.Date == request.Date && studentIds.Contains(a.StudentId) && !a.IsDeleted)
            .ToListAsync(cancellationToken);

        return _mapper.Map<List<AttendanceRecordResponse>>(allRecords);
    }
}
