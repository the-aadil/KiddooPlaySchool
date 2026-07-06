using KiddooPlaySchool.Application.DTOs.Common;
using KiddooPlaySchool.Application.DTOs.Student;
using KiddooPlaySchool.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace KiddooPlaySchool.Web.Controllers;

[ApiController]
[Route("api/students")]
public class StudentsController : ControllerBase
{
    private readonly IStudentService _studentService;

    public StudentsController(IStudentService studentService)
    {
        _studentService = studentService;
    }

    [HttpPost]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Create([FromBody] CreateStudentRequest request)
    {
        var response = await _studentService.CreateAsync(request);
        return CreatedAtAction(nameof(GetById), new { id = response.Id }, ApiResponse<StudentResponse>.Ok(response, "Student created successfully."));
    }

    [HttpGet("{id:guid}")]
    [Authorize(Roles = "Admin,Teacher")]
    public async Task<IActionResult> GetById(Guid id)
    {
        var response = await _studentService.GetByIdAsync(id);
        return Ok(ApiResponse<StudentResponse>.Ok(response));
    }

    [HttpGet]
    [Authorize(Roles = "Admin,Teacher")]
    public async Task<IActionResult> GetAll()
    {
        var response = await _studentService.GetAllAsync();
        return Ok(ApiResponse<IEnumerable<StudentResponse>>.Ok(response));
    }

    [HttpGet("by-classroom/{classRoomId:guid}")]
    [Authorize(Roles = "Admin,Teacher")]
    public async Task<IActionResult> GetByClassRoom(Guid classRoomId)
    {
        var response = await _studentService.GetByClassRoomAsync(classRoomId);
        return Ok(ApiResponse<IEnumerable<StudentResponse>>.Ok(response));
    }

    [HttpPut("{id:guid}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Update(Guid id, [FromBody] UpdateStudentRequest request)
    {
        await _studentService.UpdateAsync(id, request);
        return Ok(ApiResponse.Ok("Student updated successfully."));
    }

    [HttpDelete("{id:guid}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Delete(Guid id)
    {
        await _studentService.DeleteAsync(id);
        return Ok(ApiResponse.Ok("Student deleted successfully."));
    }
}
