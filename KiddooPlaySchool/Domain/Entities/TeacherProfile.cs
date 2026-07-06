namespace KiddooPlaySchool.Domain.Entities;

public class TeacherProfile : BaseEntity
{
    public Guid UserId { get; set; }
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

    public ApplicationUser User { get; set; } = null!;
    public ICollection<TeacherClassRoom> TeacherClassRooms { get; set; } = new List<TeacherClassRoom>();
}
