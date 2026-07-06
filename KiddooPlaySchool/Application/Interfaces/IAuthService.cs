using KiddooPlaySchool.Application.DTOs.Auth;

namespace KiddooPlaySchool.Application.Interfaces;

public interface IAuthService
{
    Task<LoginResponse> LoginAsync(LoginRequest request);
    Task<LoginResponse> RegisterAdminAsync(AdminRegisterRequest request);
}
