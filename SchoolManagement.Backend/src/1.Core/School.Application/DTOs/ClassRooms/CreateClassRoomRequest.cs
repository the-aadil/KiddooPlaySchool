using School.Domain.Enums;

namespace School.Application.DTOs.ClassRooms;

public class CreateClassRoomRequest
{
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    public int Capacity { get; set; }
    public AgeGroup AgeGroup { get; set; }
}
