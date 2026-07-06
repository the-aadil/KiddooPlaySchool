using KiddooPlaySchool.Application.DTOs.Common;
using KiddooPlaySchool.Application.DTOs.Teacher;
using KiddooPlaySchool.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace KiddooPlaySchool.Web.Controllers;

[ApiController]
[Route("api/teachers")]
[Authorize(Roles = "Admin")]
public class TeachersController : ControllerBase
{
    private readonly ITeacherService _teacherService;

    public TeachersController(ITeacherService teacherService)
    {
        _teacherService = teacherService;
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateTeacherRequest request)
    {
        var response = await _teacherService.CreateAsync(request);
        return CreatedAtAction(nameof(GetById), new { id = response.Id }, ApiResponse<TeacherResponse>.Ok(response, "Teacher created successfully."));
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetById(Guid id)
    {
        var response = await _teacherService.GetByIdAsync(id);
        return Ok(ApiResponse<TeacherResponse>.Ok(response));
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var response = await _teacherService.GetAllAsync();
        return Ok(ApiResponse<IEnumerable<TeacherResponse>>.Ok(response));
    }

    [HttpPut("{id:guid}")]
    public async Task<IActionResult> Update(Guid id, [FromBody] UpdateTeacherRequest request)
    {
        await _teacherService.UpdateAsync(id, request);
        return Ok(ApiResponse.Ok("Teacher updated successfully."));
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        await _teacherService.DeleteAsync(id);
        return Ok(ApiResponse.Ok("Teacher deleted successfully."));
    }
}
