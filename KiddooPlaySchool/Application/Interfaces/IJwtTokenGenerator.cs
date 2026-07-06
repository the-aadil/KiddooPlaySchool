using KiddooPlaySchool.Domain.Entities;

namespace KiddooPlaySchool.Application.Interfaces;

public interface IJwtTokenGenerator
{
    string GenerateToken(ApplicationUser user);
}
