using KiddooPlaySchool.Application.DTOs;

namespace KiddooPlaySchool.Application.Interfaces;

public interface ITeacherService
{
    Task<TeacherResponse> RegisterAsync(RegisterTeacherRequest request);
    Task<TeacherResponse?> GetByIdAsync(Guid id);
    Task<IEnumerable<TeacherResponse>> GetAllAsync();
    Task DeleteAsync(Guid id);
}
