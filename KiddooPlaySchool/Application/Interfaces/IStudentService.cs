using KiddooPlaySchool.Application.DTOs.Student;

namespace KiddooPlaySchool.Application.Interfaces;

public interface IStudentService
{
    Task<StudentResponse> CreateAsync(CreateStudentRequest request);
    Task<StudentResponse> GetByIdAsync(Guid id);
    Task<IEnumerable<StudentResponse>> GetAllAsync();
    Task<IEnumerable<StudentResponse>> GetByClassRoomAsync(Guid classRoomId);
    Task UpdateAsync(Guid id, UpdateStudentRequest request);
    Task DeleteAsync(Guid id);
}
