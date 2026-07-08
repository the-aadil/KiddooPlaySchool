namespace School.Application.DTOs.ClassRooms;

public class ClassRoomResponse
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    public int Capacity { get; set; }
    public string AgeGroup { get; set; } = string.Empty;
    public bool IsActive { get; set; }
    public int StudentCount { get; set; }
    public DateTimeOffset CreatedAt { get; set; }
}
