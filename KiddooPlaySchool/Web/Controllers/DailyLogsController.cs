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
[Route("api/daily-logs")]
public class DailyLogsController : ControllerBase
{
    private readonly IDailyLogService _dailyLogService;
    private readonly AppDbContext _context;

    public DailyLogsController(IDailyLogService dailyLogService, AppDbContext context)
    {
        _dailyLogService = dailyLogService;
        _context = context;
    }

    [HttpPost]
    [Authorize(Roles = "Teacher")]
    public async Task<IActionResult> Create([FromBody] CreateDailyLogRequest request)
    {
        var userId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

        var teacherProfile = await _context.TeacherProfiles
            .FirstOrDefaultAsync(t => t.UserId == userId && !t.IsDeleted)
            ?? throw new UnauthorizedAccessException("Teacher profile not found for the current user.");

        var result = await _dailyLogService.CreateAsync(request, teacherProfile.Id);
        return CreatedAtAction(nameof(GetStudentFeed), new { studentId = request.StudentId },
            ApiResponse<DailyLogResponse>.Ok(result, "Daily activity log created successfully."));
    }

    [HttpGet("student/{studentId:guid}")]
    [Authorize(Roles = "Admin,Teacher,Student")]
    public async Task<IActionResult> GetStudentFeed(Guid studentId, [FromQuery] DateOnly? date)
    {
        var userId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
        var userRole = User.FindFirstValue(ClaimTypes.Role) ?? string.Empty;

        if (userRole == "Student")
        {
            var studentProfile = await _context.StudentProfiles
                .FirstOrDefaultAsync(s => s.UserId == userId && !s.IsDeleted);

            if (studentProfile is null || studentProfile.Id != studentId)
            {
                return Forbid();
            }
        }

        IEnumerable<DailyLogResponse> result;

        if (date.HasValue)
        {
            result = await _dailyLogService.GetStudentLogsByDateAsync(studentId, date.Value, userId, userRole);
        }
        else
        {
            result = await _dailyLogService.GetStudentFeedAsync(studentId, userId, userRole);
        }

        return Ok(ApiResponse<IEnumerable<DailyLogResponse>>.Ok(result));
    }
}
