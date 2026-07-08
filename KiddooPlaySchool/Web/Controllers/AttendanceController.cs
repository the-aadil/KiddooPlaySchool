using System.Security.Claims;
using KiddooPlaySchool.Application.DTOs.Attendance;
using KiddooPlaySchool.Application.DTOs.Common;
using KiddooPlaySchool.Application.Interfaces;
using KiddooPlaySchool.Infrastructure.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace KiddooPlaySchool.Web.Controllers;

[ApiController]
[Route("api/attendance")]
public class AttendanceController : ControllerBase
{
    private readonly IAttendanceService _attendanceService;
    private readonly AppDbContext _context;

    public AttendanceController(IAttendanceService attendanceService, AppDbContext context)
    {
        _attendanceService = attendanceService;
        _context = context;
    }

    [HttpPost("bulk")]
    [Authorize(Roles = "Admin,Teacher")]
    public async Task<IActionResult> RecordBulkAttendance([FromBody] RecordBulkAttendanceRequest request)
    {
        var userId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
        var userRole = User.FindFirstValue(ClaimTypes.Role);

        if (userRole == "Teacher")
        {
            var teacherProfile = await _context.TeacherProfiles
                .FirstOrDefaultAsync(t => t.UserId == userId && !t.IsDeleted);

            if (teacherProfile is null)
            {
                return Forbid();
            }

            var isAssigned = await _context.TeacherClassRooms
                .AnyAsync(tc => tc.TeacherProfileId == teacherProfile.Id
                             && tc.ClassRoomId == request.ClassRoomId);

            if (!isAssigned)
            {
                return Forbid();
            }
        }

        var result = await _attendanceService.RecordBulkAttendanceAsync(request, userId);
        return Ok(ApiResponse<List<AttendanceRecordResponse>>.Ok(result, "Attendance recorded successfully."));
    }

    [HttpGet("student/{studentId:guid}")]
    [Authorize(Roles = "Admin,Teacher,Student")]
    public async Task<IActionResult> GetStudentAttendance(Guid studentId, [FromQuery] DateOnly? date)
    {
        var userId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
        var userRole = User.FindFirstValue(ClaimTypes.Role);

        if (userRole == "Student")
        {
            var studentProfile = await _context.StudentProfiles
                .FirstOrDefaultAsync(s => s.UserId == userId && !s.IsDeleted);

            if (studentProfile is null || studentProfile.Id != studentId)
            {
                return Forbid();
            }
        }

        if (date.HasValue)
        {
            var result = await _attendanceService.GetStudentAttendanceAsync(studentId, date.Value);
            return result is null
                ? Ok(ApiResponse<AttendanceRecordResponse?>.Ok(null, "No attendance record found."))
                : Ok(ApiResponse<AttendanceRecordResponse>.Ok(result));
        }

        var rangeStart = DateOnly.FromDateTime(DateTime.UtcNow.AddDays(-30));
        var rangeEnd = DateOnly.FromDateTime(DateTime.UtcNow);
        var records = await _attendanceService.GetStudentAttendanceRangeAsync(studentId, rangeStart, rangeEnd);
        return Ok(ApiResponse<IEnumerable<AttendanceRecordResponse>>.Ok(records));
    }

    [HttpGet("classroom/{classRoomId:guid}")]
    [Authorize(Roles = "Admin,Teacher")]
    public async Task<IActionResult> GetClassRoomAttendance(Guid classRoomId, [FromQuery] DateOnly date)
    {
        var userId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
        var userRole = User.FindFirstValue(ClaimTypes.Role);

        if (userRole == "Teacher")
        {
            var teacherProfile = await _context.TeacherProfiles
                .FirstOrDefaultAsync(t => t.UserId == userId && !t.IsDeleted);

            if (teacherProfile is null)
            {
                return Forbid();
            }

            var isAssigned = await _context.TeacherClassRooms
                .AnyAsync(tc => tc.TeacherProfileId == teacherProfile.Id
                             && tc.ClassRoomId == classRoomId);

            if (!isAssigned)
            {
                return Forbid();
            }
        }

        var records = await _attendanceService.GetClassRoomAttendanceAsync(classRoomId, date);
        return Ok(ApiResponse<IEnumerable<AttendanceRecordResponse>>.Ok(records));
    }
}
