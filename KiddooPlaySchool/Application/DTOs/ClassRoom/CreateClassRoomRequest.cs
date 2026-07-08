using KiddooPlaySchool.Domain.Enums;

namespace KiddooPlaySchool.Application.DTOs.ClassRoom;

public class CreateClassRoomRequest
{
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    public int Capacity { get; set; }
    public AgeGroup AgeGroup { get; set; }
}
