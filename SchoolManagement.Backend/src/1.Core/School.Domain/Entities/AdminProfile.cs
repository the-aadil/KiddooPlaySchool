using School.Domain.Common;

namespace School.Domain.Entities;

public class AdminProfile : BaseAuditableEntity
{
    public Guid UserId { get; set; }
    public string Department { get; set; } = string.Empty;
    public string? Designation { get; set; }

    public ApplicationUser User { get; set; } = null!;
}
