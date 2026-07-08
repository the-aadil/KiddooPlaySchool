using KiddooPlaySchool.Application.DTOs.ClassRoom;
using KiddooPlaySchool.Application.DTOs.Common;
using KiddooPlaySchool.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace KiddooPlaySchool.Web.Controllers;

[ApiController]
[Route("api/classrooms")]
public class ClassRoomsController : ControllerBase
{
    private readonly IClassRoomService _classRoomService;

    public ClassRoomsController(IClassRoomService classRoomService)
    {
        _classRoomService = classRoomService;
    }

    [HttpPost]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Create([FromBody] CreateClassRoomRequest request)
    {
        var response = await _classRoomService.CreateAsync(request);
        return CreatedAtAction(nameof(GetById), new { id = response.Id }, ApiResponse<ClassRoomResponse>.Ok(response, "Class room created successfully."));
    }

    [HttpGet("{id:guid}")]
    [Authorize(Roles = "Admin,Teacher")]
    public async Task<IActionResult> GetById(Guid id)
    {
        var response = await _classRoomService.GetByIdAsync(id);
        return Ok(ApiResponse<ClassRoomResponse>.Ok(response));
    }

    [HttpGet]
    [Authorize(Roles = "Admin,Teacher")]
    public async Task<IActionResult> GetAll()
    {
        var response = await _classRoomService.GetAllAsync();
        return Ok(ApiResponse<IEnumerable<ClassRoomResponse>>.Ok(response));
    }

    [HttpPut("{id:guid}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Update(Guid id, [FromBody] UpdateClassRoomRequest request)
    {
        await _classRoomService.UpdateAsync(id, request);
        return Ok(ApiResponse.Ok("Class room updated successfully."));
    }

    [HttpDelete("{id:guid}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Delete(Guid id)
    {
        await _classRoomService.DeleteAsync(id);
        return Ok(ApiResponse.Ok("Class room deleted successfully."));
    }

    [HttpPost("assign-teacher")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> AssignTeacher([FromBody] AssignTeacherRequest request)
    {
        await _classRoomService.AssignTeacherAsync(request);
        return Ok(ApiResponse.Ok("Teacher assigned to classroom successfully."));
    }

    [HttpDelete("assign-teacher/{teacherProfileId:guid}/{classRoomId:guid}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> RemoveTeacherAssignment(Guid teacherProfileId, Guid classRoomId)
    {
        await _classRoomService.RemoveTeacherAssignmentAsync(teacherProfileId, classRoomId);
        return Ok(ApiResponse.Ok("Teacher removed from classroom successfully."));
    }

    [HttpGet("teacher/{teacherProfileId:guid}")]
    [Authorize(Roles = "Admin,Teacher")]
    public async Task<IActionResult> GetTeacherClassRooms(Guid teacherProfileId)
    {
        var result = await _classRoomService.GetTeacherClassRoomsAsync(teacherProfileId);
        return Ok(ApiResponse<IEnumerable<ClassRoomResponse>>.Ok(result));
    }
}
