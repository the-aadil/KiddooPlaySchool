namespace KiddooPlaySchool.Domain.Entities;

public class Teacher : BaseEntity
{
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string Username { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string PasswordHash { get; set; } = string.Empty;
    public string MobileNumber { get; set; } = string.Empty;
    public string? AlternateMobile { get; set; }
    public DateTime DateOfBirth { get; set; }
    public string Gender { get; set; } = string.Empty;
    public string Qualification { get; set; } = string.Empty;
    public string? Specialization { get; set; }
    public string? Address { get; set; }
    public string? City { get; set; }
    public string? State { get; set; }
    public DateTime JoinDate { get; set; }
    public string EmployeeId { get; set; } = string.Empty;
    public bool IsActive { get; set; } = true;
}
