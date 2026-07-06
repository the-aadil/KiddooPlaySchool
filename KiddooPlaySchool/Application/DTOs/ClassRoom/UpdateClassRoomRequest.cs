namespace KiddooPlaySchool.Application.DTOs.ClassRoom;

public class UpdateClassRoomRequest
{
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    public int Capacity { get; set; }
}
