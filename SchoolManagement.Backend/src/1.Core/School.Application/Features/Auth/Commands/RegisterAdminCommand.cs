using MediatR;
using School.Application.DTOs.Auth;

namespace School.Application.Features.Auth.Commands;

public class RegisterAdminCommand : IRequest<AuthResponse>
{
    public string Username { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string MobileNumber { get; set; } = string.Empty;
    public string Department { get; set; } = string.Empty;
    public string? Designation { get; set; }
}
