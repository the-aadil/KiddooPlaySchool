using KiddooPlaySchool.Application.DTOs;
using KiddooPlaySchool.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace KiddooPlaySchool.Web.Controllers;

[ApiController]
[Route("api/[controller]")]
public class TeachersController : ControllerBase
{
    private readonly ITeacherService _teacherService;

    public TeachersController(ITeacherService teacherService)
    {
        _teacherService = teacherService;
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegisterTeacherRequest request)
    {
        var teacher = await _teacherService.RegisterAsync(request);
        return CreatedAtAction(nameof(GetById), new { id = teacher.Id }, teacher);
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetById(Guid id)
    {
        var teacher = await _teacherService.GetByIdAsync(id);
        return teacher is null ? NotFound() : Ok(teacher);
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var teachers = await _teacherService.GetAllAsync();
        return Ok(teachers);
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        await _teacherService.DeleteAsync(id);
        return NoContent();
    }
}
