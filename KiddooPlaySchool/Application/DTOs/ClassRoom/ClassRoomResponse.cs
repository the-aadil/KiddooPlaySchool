namespace KiddooPlaySchool.Application.DTOs.ClassRoom;

public class ClassRoomResponse
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    public int Capacity { get; set; }
    public string AgeGroup { get; set; } = string.Empty;
    public bool IsActive { get; set; }
    public int StudentCount { get; set; }
    public DateTime CreatedAt { get; set; }
}
