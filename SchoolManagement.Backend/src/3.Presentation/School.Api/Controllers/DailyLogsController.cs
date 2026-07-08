using System.Security.Claims;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using School.Application.DTOs.Common;
using School.Application.DTOs.DailyLogs;
using School.Application.Features.DailyLogs.Commands;
using School.Application.Features.DailyLogs.Queries;
using School.Domain.Enums;
using School.Infrastructure.Persistence;

namespace School.Api.Controllers;

[ApiController]
[Route("api/daily-logs")]
public class DailyLogsController : ControllerBase
{
    private readonly IMediator _mediator;
    private readonly ApplicationDbContext _context;

    public DailyLogsController(IMediator mediator, ApplicationDbContext context)
    {
        _mediator = mediator;
        _context = context;
    }

    [HttpPost]
    [Authorize(Roles = "Teacher")]
    public async Task<IActionResult> Create([FromBody] CreateDailyLogCommand command)
    {
        var response = await _mediator.Send(command);
        return CreatedAtAction(nameof(GetStudentFeed), new { studentId = command.StudentId },
            ApiResponse<DailyLogResponse>.Ok(response, "Daily activity log created successfully."));
    }

    [HttpGet("student/{studentId:guid}")]
    [Authorize(Roles = "Admin,Teacher,Student")]
    public async Task<IActionResult> GetStudentFeed(
        Guid studentId,
        [FromQuery] GetStudentDailyLogsQuery query)
    {
        var userId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
        var userRole = User.FindFirstValue(ClaimTypes.Role);

        // Student boundary: only their own feed
        if (userRole == UserRole.Student)
        {
            var studentProfile = await _context.StudentProfiles
                .AsNoTracking()
                .FirstOrDefaultAsync(s => s.UserId == userId && !s.IsDeleted);

            if (studentProfile is null || studentProfile.Id != studentId)
                return Forbid();
        }

        // Teacher boundary: only students in assigned classrooms
        if (userRole == UserRole.Teacher)
        {
            var teacherProfile = await _context.TeacherProfiles
                .AsNoTracking()
                .FirstOrDefaultAsync(t => t.UserId == userId && !t.IsDeleted);

            if (teacherProfile is null)
                return Forbid();

            var student = await _context.StudentProfiles
                .AsNoTracking()
                .FirstOrDefaultAsync(s => s.Id == studentId && !s.IsDeleted);

            if (student is null || !student.ClassRoomId.HasValue)
                return Forbid();

            var isAssigned = await _context.TeacherClassRoomAssignments
                .AsNoTracking()
                .AnyAsync(tc => tc.TeacherProfileId == teacherProfile.Id
                             && tc.ClassRoomId == student.ClassRoomId);

            if (!isAssigned)
                return Forbid();
        }

        query.StudentId = studentId;
        var result = await _mediator.Send(query);
        return Ok(ApiResponse<PagedResponse<DailyLogResponse>>.Ok(result));
    }
}
