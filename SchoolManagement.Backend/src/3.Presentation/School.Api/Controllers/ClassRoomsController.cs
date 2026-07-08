using System.Security.Claims;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using School.Application.DTOs.ClassRooms;
using School.Application.DTOs.Common;
using School.Application.Features.ClassRooms.Commands;
using School.Application.Features.ClassRooms.Queries;
using School.Domain.Enums;
using School.Infrastructure.Persistence;

namespace School.Api.Controllers;

[ApiController]
[Route("api/classrooms")]
public class ClassRoomsController : ControllerBase
{
    private readonly IMediator _mediator;
    private readonly ApplicationDbContext _context;

    public ClassRoomsController(IMediator mediator, ApplicationDbContext context)
    {
        _mediator = mediator;
        _context = context;
    }

    [HttpPost]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Create([FromBody] CreateClassRoomCommand command)
    {
        var response = await _mediator.Send(command);
        return CreatedAtAction(nameof(GetById), new { id = response.Id },
            ApiResponse<ClassRoomResponse>.Ok(response, "Class room created successfully."));
    }

    [HttpGet("{id:guid}")]
    [Authorize(Roles = "Admin,Teacher")]
    public async Task<IActionResult> GetById(Guid id)
    {
        // Teacher boundary: only assigned classrooms
        if (User.IsInRole(UserRole.Teacher))
        {
            var userId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
            var teacherProfile = await _context.TeacherProfiles
                .AsNoTracking()
                .FirstOrDefaultAsync(t => t.UserId == userId && !t.IsDeleted);

            if (teacherProfile is null)
                return Forbid();

            var isAssigned = await _context.TeacherClassRoomAssignments
                .AsNoTracking()
                .AnyAsync(tc => tc.TeacherProfileId == teacherProfile.Id && tc.ClassRoomId == id);

            if (!isAssigned)
                return Forbid();
        }

        var response = await _mediator.Send(new GetClassRoomByIdQuery { Id = id });
        return Ok(ApiResponse<ClassRoomResponse>.Ok(response));
    }

    [HttpGet]
    [Authorize(Roles = "Admin,Teacher")]
    public async Task<IActionResult> GetAll([FromQuery] GetClassRoomsQuery query)
    {
        var response = await _mediator.Send(query);
        return Ok(ApiResponse<PagedResponse<ClassRoomResponse>>.Ok(response));
    }

    [HttpPost("assign-teacher")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> AssignTeacher([FromBody] AssignTeacherCommand command)
    {
        await _mediator.Send(command);
        return Ok(ApiResponse.Ok("Teacher assigned to classroom successfully."));
    }
}
