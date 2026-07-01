using KiddooPlaySchool.Application.DTOs;
using KiddooPlaySchool.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace KiddooPlaySchool.Web.Controllers;

[ApiController]
[Route("api/[controller]")]
public class StudentsController : ControllerBase
{
    private readonly IStudentService _studentService;

    public StudentsController(IStudentService studentService)
    {
        _studentService = studentService;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var students = await _studentService.GetAllAsync();
        return Ok(students);
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetById(Guid id)
    {
        var student = await _studentService.GetByIdAsync(id);
        return student is null ? NotFound() : Ok(student);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] StudentDto student)
    {
        await _studentService.CreateAsync(student);
        return CreatedAtAction(nameof(GetById), new { id = student.Id }, student);
    }

    [HttpPut("{id:guid}")]
    public async Task<IActionResult> Update(Guid id, [FromBody] StudentDto student)
    {
        if (id != student.Id) return BadRequest("Id mismatch");
        await _studentService.UpdateAsync(student);
        return NoContent();
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        await _studentService.DeleteAsync(id);
        return NoContent();
    }
}
