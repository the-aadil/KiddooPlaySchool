using KiddooPlaySchool.Application.DTOs;

namespace KiddooPlaySchool.Application.Interfaces;

public interface IStudentService
{
    Task<IEnumerable<StudentDto>> GetAllAsync();
    Task<StudentDto?> GetByIdAsync(Guid id);
    Task<Guid> CreateAsync(CreateStudentRequest request);
    Task UpdateAsync(StudentDto student);
    Task DeleteAsync(Guid id);
}
