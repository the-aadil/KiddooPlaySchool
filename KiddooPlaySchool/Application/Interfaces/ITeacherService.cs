using KiddooPlaySchool.Application.DTOs.Teacher;

namespace KiddooPlaySchool.Application.Interfaces;

public interface ITeacherService
{
    Task<TeacherResponse> CreateAsync(CreateTeacherRequest request);
    Task<TeacherResponse> GetByIdAsync(Guid id);
    Task<IEnumerable<TeacherResponse>> GetAllAsync();
    Task UpdateAsync(Guid id, UpdateTeacherRequest request);
    Task DeleteAsync(Guid id);
}
