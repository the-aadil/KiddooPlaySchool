using KiddooPlaySchool.Application.DTOs.ClassRoom;

namespace KiddooPlaySchool.Application.Interfaces;

public interface IClassRoomService
{
    Task<ClassRoomResponse> CreateAsync(CreateClassRoomRequest request);
    Task<ClassRoomResponse> GetByIdAsync(Guid id);
    Task<IEnumerable<ClassRoomResponse>> GetAllAsync();
    Task UpdateAsync(Guid id, UpdateClassRoomRequest request);
    Task DeleteAsync(Guid id);
}
