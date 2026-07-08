using AutoMapper;
using KiddooPlaySchool.Application.DTOs.Attendance;
using KiddooPlaySchool.Application.Interfaces;
using KiddooPlaySchool.Domain.Entities;
using KiddooPlaySchool.Domain.Enums;
using KiddooPlaySchool.Domain.Exceptions;
using KiddooPlaySchool.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace KiddooPlaySchool.Application.Services;

public class DailyLogService : IDailyLogService
{
    private readonly IRepository<DailyActivityLog> _logRepository;
    private readonly IRepository<AttendanceRecord> _attendanceRepository;
    private readonly IRepository<StudentProfile> _studentRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public DailyLogService(
        IRepository<DailyActivityLog> logRepository,
        IRepository<AttendanceRecord> attendanceRepository,
        IRepository<StudentProfile> studentRepository,
        IUnitOfWork unitOfWork,
        IMapper mapper)
    {
        _logRepository = logRepository;
        _attendanceRepository = attendanceRepository;
        _studentRepository = studentRepository;
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<DailyLogResponse> CreateAsync(CreateDailyLogRequest request, Guid teacherProfileId)
    {
        var logDate = DateOnly.FromDateTime(request.LogTimestamp.DateTime);

        var attendance = await _attendanceRepository.GetAll()
            .FirstOrDefaultAsync(a => a.StudentId == request.StudentId
                                   && a.Date == logDate
                                   && !a.IsDeleted);

        if (attendance is null || attendance.Status != AttendanceStatus.Present)
        {
            throw new DomainInvariantViolationException(
                "Daily activity log can only be created for a student whose attendance status for that day is 'Present'.");
        }

        var log = new DailyActivityLog
        {
            StudentId = request.StudentId,
            TeacherId = teacherProfileId,
            LogTimestamp = request.LogTimestamp,
            ActivityType = request.ActivityType,
            Payload = request.Payload ?? string.Empty,
            Visibility = request.Visibility,
            Remarks = request.Remarks,
            MediaUrls = request.MediaUrls
        };

        await _logRepository.AddAsync(log);
        await _unitOfWork.SaveChangesAsync();

        var savedLog = await _logRepository.GetAll()
            .Include(d => d.Student).ThenInclude(s => s.User)
            .Include(d => d.Teacher).ThenInclude(t => t.User)
            .FirstAsync(d => d.Id == log.Id);

        return _mapper.Map<DailyLogResponse>(savedLog);
    }

    public async Task<IEnumerable<DailyLogResponse>> GetStudentFeedAsync(
        Guid studentId, Guid? currentUserId, string currentUserRole)
    {
        var query = _logRepository.GetAll()
            .Include(d => d.Student).ThenInclude(s => s.User)
            .Include(d => d.Teacher).ThenInclude(t => t.User)
            .Where(d => d.StudentId == studentId && !d.IsDeleted);

        if (currentUserRole == UserRole.Student)
        {
            query = query.Where(d => d.Visibility == ActivityVisibility.SharedWithParent);
        }

        var logs = await query
            .OrderByDescending(d => d.LogTimestamp)
            .ToListAsync();

        return _mapper.Map<IEnumerable<DailyLogResponse>>(logs);
    }

    public async Task<IEnumerable<DailyLogResponse>> GetStudentLogsByDateAsync(
        Guid studentId, DateOnly date, Guid? currentUserId, string currentUserRole)
    {
        var query = _logRepository.GetAll()
            .Include(d => d.Student).ThenInclude(s => s.User)
            .Include(d => d.Teacher).ThenInclude(t => t.User)
            .Where(d => d.StudentId == studentId
                     && DateOnly.FromDateTime(d.LogTimestamp.DateTime) == date
                     && !d.IsDeleted);

        if (currentUserRole == UserRole.Student)
        {
            query = query.Where(d => d.Visibility == ActivityVisibility.SharedWithParent);
        }

        var logs = await query
            .OrderByDescending(d => d.LogTimestamp)
            .ToListAsync();

        return _mapper.Map<IEnumerable<DailyLogResponse>>(logs);
    }
}
