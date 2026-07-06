namespace KiddooPlaySchool.Application.DTOs.Teacher;

public class UpdateTeacherRequest
{
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string MobileNumber { get; set; } = string.Empty;
    public string? Password { get; set; }
    public string? AlternateMobile { get; set; }
    public DateTime DateOfBirth { get; set; }
    public string Gender { get; set; } = string.Empty;
    public string Qualification { get; set; } = string.Empty;
    public string? Specialization { get; set; }
    public string? Address { get; set; }
    public string? City { get; set; }
    public string? State { get; set; }
}
