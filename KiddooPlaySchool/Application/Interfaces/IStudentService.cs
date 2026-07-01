using KiddooPlaySchool.Application.DTOs;

namespace KiddooPlaySchool.Application.Interfaces;

public interface IStudentService
{
    Task<IEnumerable<StudentDto>> GetAllAsync();
    Task<StudentDto?> GetByIdAsync(Guid id);
    Task CreateAsync(StudentDto student);
    Task UpdateAsync(StudentDto student);
    Task DeleteAsync(Guid id);
}
