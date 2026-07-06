namespace KiddooPlaySchool.Application.DTOs.Student;

public class StudentResponse
{
    public Guid Id { get; set; }
    public Guid UserId { get; set; }
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string FullName => $"{FirstName} {LastName}";
    public string Email { get; set; } = string.Empty;
    public string Phone { get; set; } = string.Empty;
    public DateTime DateOfBirth { get; set; }
    public string? Address { get; set; }
    public string? ParentName { get; set; }
    public string? ParentPhone { get; set; }
    public string? ParentEmail { get; set; }
    public DateTime EnrollmentDate { get; set; }
    public string? ClassRoomName { get; set; }
    public Guid? ClassRoomId { get; set; }
    public DateTime CreatedAt { get; set; }
}
