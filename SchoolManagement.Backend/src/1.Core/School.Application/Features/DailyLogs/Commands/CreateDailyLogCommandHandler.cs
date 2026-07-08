using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using School.Application.DTOs.DailyLogs;
using School.Application.Interfaces;
using School.Domain.Entities;
using School.Domain.Enums;
using School.Domain.Exceptions;

namespace School.Application.Features.DailyLogs.Commands;

public class CreateDailyLogCommandHandler : IRequestHandler<CreateDailyLogCommand, DailyLogResponse>
{
    private readonly IApplicationDbContext _context;
    private readonly ICurrentUserContext _currentUser;
    private readonly IMapper _mapper;

    public CreateDailyLogCommandHandler(
        IApplicationDbContext context,
        ICurrentUserContext currentUser,
        IMapper mapper)
    {
        _context = context;
        _currentUser = currentUser;
        _mapper = mapper;
    }

    public async Task<DailyLogResponse> Handle(
        CreateDailyLogCommand request, CancellationToken cancellationToken)
    {
        // FIX 7: Defense-in-depth - block future-dated logs beyond current UTC
        if (request.LogTimestamp > DateTimeOffset.UtcNow.AddHours(1))
        {
            throw new DomainException("Daily activity log cannot be recorded for future dates.");
        }

        var teacherProfile = await _context.TeacherProfiles
            .AsNoTracking()
            .FirstOrDefaultAsync(t => t.UserId == _currentUser.UserId && !t.IsDeleted, cancellationToken)
            ?? throw new DomainException("Teacher profile not found for the current user.");

        var student = await _context.StudentProfiles
            .AsNoTracking()
            .Include(s => s.ClassRoom)
            .FirstOrDefaultAsync(s => s.Id == request.StudentId && !s.IsDeleted, cancellationToken)
            ?? throw new DomainException($"Student with ID {request.StudentId} was not found.");

        // FIX 6: Teacher-classroom boundary enforcement
        if (student.ClassRoomId.HasValue)
        {
            var isAssigned = await _context.TeacherClassRoomAssignments
                .AsNoTracking()
                .AnyAsync(tc => tc.TeacherProfileId == teacherProfile.Id
                             && tc.ClassRoomId == student.ClassRoomId, cancellationToken);

            if (!isAssigned)
            {
                throw new DomainException(
                    "You are not assigned to the classroom this student belongs to.");
            }
        }

        var logDate = DateOnly.FromDateTime(request.LogTimestamp.DateTime);

        var attendance = await _context.AttendanceRecords
            .AsNoTracking()
            .FirstOrDefaultAsync(a => a.StudentId == request.StudentId
                                  && a.Date == logDate
                                  && !a.IsDeleted, cancellationToken);

        if (attendance is null || attendance.Status != AttendanceStatus.Present)
        {
            throw new DomainException(
                "Daily activity log can only be created for a student whose attendance status for that day is 'Present'.");
        }

        var log = new DailyActivityLog
        {
            StudentId = request.StudentId,
            TeacherId = teacherProfile.Id,
            LogTimestamp = request.LogTimestamp,
            ActivityType = request.ActivityType,
            Payload = request.Payload ?? string.Empty,
            Visibility = request.Visibility,
            Remarks = request.Remarks,
            MediaUrls = request.MediaUrls ?? []
        };

        _context.DailyActivityLogs.Add(log);
        await _context.SaveChangesAsync(cancellationToken);

        var savedLog = await _context.DailyActivityLogs
            .Include(d => d.Student).ThenInclude(s => s.User)
            .Include(d => d.Teacher).ThenInclude(t => t.User)
            .FirstAsync(d => d.Id == log.Id, cancellationToken);

        return _mapper.Map<DailyLogResponse>(savedLog);
    }
}
