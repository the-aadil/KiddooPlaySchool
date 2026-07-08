using System.Security.Claims;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using School.Application.DTOs.Attendance;
using School.Application.DTOs.Common;
using School.Application.Features.Attendance.Commands;
using School.Application.Features.Attendance.Queries;
using School.Domain.Enums;
using School.Infrastructure.Persistence;

namespace School.Api.Controllers;

[ApiController]
[Route("api/attendance")]
public class AttendanceController : ControllerBase
{
    private readonly IMediator _mediator;
    private readonly ApplicationDbContext _context;

    public AttendanceController(IMediator mediator, ApplicationDbContext context)
    {
        _mediator = mediator;
        _context = context;
    }

    [HttpPost("bulk")]
    [Authorize(Roles = "Admin,Teacher")]
    public async Task<IActionResult> RecordBulkAttendance([FromBody] RecordBulkAttendanceCommand command)
    {
        var userRole = User.FindFirstValue(ClaimTypes.Role);

        // FIX 5: Teacher-classroom boundary enforcement
        if (userRole == UserRole.Teacher)
        {
            var userId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
            var teacherProfile = await _context.TeacherProfiles
                .AsNoTracking()
                .FirstOrDefaultAsync(t => t.UserId == userId && !t.IsDeleted);

            if (teacherProfile is null)
                return Forbid();

            var isAssigned = await _context.TeacherClassRoomAssignments
                .AsNoTracking()
                .AnyAsync(tc => tc.TeacherProfileId == teacherProfile.Id
                             && tc.ClassRoomId == command.ClassRoomId);

            if (!isAssigned)
                return Forbid();
        }

        var result = await _mediator.Send(command);
        return Ok(ApiResponse<List<AttendanceRecordResponse>>.Ok(result, "Attendance recorded successfully."));
    }

    [HttpGet("classroom/{classRoomId:guid}")]
    [Authorize(Roles = "Admin,Teacher")]
    public async Task<IActionResult> GetClassRoomAttendance(Guid classRoomId, [FromQuery] DateOnly date)
    {
        var userRole = User.FindFirstValue(ClaimTypes.Role);

        // FIX 5: Teacher-classroom boundary enforcement on read
        if (userRole == UserRole.Teacher)
        {
            var userId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
            var teacherProfile = await _context.TeacherProfiles
                .AsNoTracking()
                .FirstOrDefaultAsync(t => t.UserId == userId && !t.IsDeleted);

            if (teacherProfile is null)
                return Forbid();

            var isAssigned = await _context.TeacherClassRoomAssignments
                .AsNoTracking()
                .AnyAsync(tc => tc.TeacherProfileId == teacherProfile.Id
                             && tc.ClassRoomId == classRoomId);

            if (!isAssigned)
                return Forbid();
        }

        var query = new GetClassRoomAttendanceQuery { ClassRoomId = classRoomId, Date = date };
        var result = await _mediator.Send(query);
        return Ok(ApiResponse<List<AttendanceRecordResponse>>.Ok(result));
    }
}
