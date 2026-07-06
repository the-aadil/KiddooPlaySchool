namespace KiddooPlaySchool.Domain.Entities;

public class AdminProfile : BaseEntity
{
    public Guid UserId { get; set; }
    public string Department { get; set; } = string.Empty;
    public string? Designation { get; set; }

    public ApplicationUser User { get; set; } = null!;
}
