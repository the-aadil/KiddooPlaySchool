using AutoMapper;
using KiddooPlaySchool.Application.DTOs.Attendance;
using KiddooPlaySchool.Application.Interfaces;
using KiddooPlaySchool.Domain.Entities;
using KiddooPlaySchool.Domain.Exceptions;
using KiddooPlaySchool.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace KiddooPlaySchool.Application.Services;

public class AttendanceService : IAttendanceService
{
    private readonly IRepository<AttendanceRecord> _attendanceRepository;
    private readonly IRepository<StudentProfile> _studentRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public AttendanceService(
        IRepository<AttendanceRecord> attendanceRepository,
        IRepository<StudentProfile> studentRepository,
        IUnitOfWork unitOfWork,
        IMapper mapper)
    {
        _attendanceRepository = attendanceRepository;
        _studentRepository = studentRepository;
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<List<AttendanceRecordResponse>> RecordBulkAttendanceAsync(
        RecordBulkAttendanceRequest request, Guid recordedById)
    {
        if (request.Date > DateOnly.FromDateTime(DateTime.UtcNow))
        {
            throw new DomainInvariantViolationException("Attendance cannot be recorded for future dates.");
        }

        var studentIds = request.Students.Select(s => s.StudentId).ToList();

        var students = await _studentRepository.GetAll()
            .Include(s => s.User)
            .Where(s => studentIds.Contains(s.Id) && s.ClassRoomId == request.ClassRoomId && !s.IsDeleted)
            .ToListAsync();

        var invalidStudents = studentIds.Except(students.Select(s => s.Id)).ToList();
        if (invalidStudents.Count != 0)
        {
            throw new EntityNotFoundException(
                $"Students with IDs [{string.Join(", ", invalidStudents)}] were not found in the specified classroom.");
        }

        var existingRecords = await _attendanceRepository.GetAll()
            .Where(a => a.Date == request.Date && studentIds.Contains(a.StudentId) && !a.IsDeleted)
            .ToListAsync();

        var results = new List<AttendanceRecordResponse>();

        foreach (var item in request.Students)
        {
            if (item.CheckInTime.HasValue && item.CheckOutTime.HasValue
                && item.CheckOutTime <= item.CheckInTime)
            {
                throw new DomainInvariantViolationException(
                    $"Check-Out time cannot be earlier than Check-In time for student {item.StudentId}.");
            }

            var existing = existingRecords.FirstOrDefault(a => a.StudentId == item.StudentId);

            if (existing != null)
            {
                existing.CheckInTime = item.CheckInTime;
                existing.CheckOutTime = item.CheckOutTime;
                existing.Status = item.Status;
                existing.Notes = item.Notes;
                existing.RecordedById = recordedById;
            }
            else
            {
                var record = new AttendanceRecord
                {
                    StudentId = item.StudentId,
                    Date = request.Date,
                    CheckInTime = item.CheckInTime,
                    CheckOutTime = item.CheckOutTime,
                    Status = item.Status,
                    Notes = item.Notes,
                    RecordedById = recordedById
                };

                await _attendanceRepository.AddAsync(record);
            }
        }

        await _unitOfWork.SaveChangesAsync();

        var allRecords = await _attendanceRepository.GetAll()
            .Include(a => a.Student).ThenInclude(s => s.User)
            .Where(a => a.Date == request.Date && studentIds.Contains(a.StudentId) && !a.IsDeleted)
            .ToListAsync();

        return _mapper.Map<List<AttendanceRecordResponse>>(allRecords);
    }

    public async Task<AttendanceRecordResponse?> GetStudentAttendanceAsync(Guid studentId, DateOnly date)
    {
        var record = await _attendanceRepository.GetAll()
            .Include(a => a.Student).ThenInclude(s => s.User)
            .FirstOrDefaultAsync(a => a.StudentId == studentId && a.Date == date && !a.IsDeleted);

        return record is null ? null : _mapper.Map<AttendanceRecordResponse>(record);
    }

    public async Task<IEnumerable<AttendanceRecordResponse>> GetClassRoomAttendanceAsync(
        Guid classRoomId, DateOnly date)
    {
        var records = await _attendanceRepository.GetAll()
            .Include(a => a.Student).ThenInclude(s => s.User)
            .Where(a => a.Student.ClassRoomId == classRoomId && a.Date == date && !a.IsDeleted)
            .ToListAsync();

        return _mapper.Map<IEnumerable<AttendanceRecordResponse>>(records);
    }

    public async Task<IEnumerable<AttendanceRecordResponse>> GetStudentAttendanceRangeAsync(
        Guid studentId, DateOnly from, DateOnly to)
    {
        var records = await _attendanceRepository.GetAll()
            .Include(a => a.Student).ThenInclude(s => s.User)
            .Where(a => a.StudentId == studentId
                     && a.Date >= from
                     && a.Date <= to
                     && !a.IsDeleted)
            .OrderBy(a => a.Date)
            .ToListAsync();

        return _mapper.Map<IEnumerable<AttendanceRecordResponse>>(records);
    }
}
