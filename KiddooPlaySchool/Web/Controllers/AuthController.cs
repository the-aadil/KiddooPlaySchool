using KiddooPlaySchool.Application.DTOs.Auth;
using KiddooPlaySchool.Application.DTOs.Common;
using KiddooPlaySchool.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;

namespace KiddooPlaySchool.Web.Controllers;

[ApiController]
[Route("api/auth")]
[EnableRateLimiting("Login")]
public class AuthController : ControllerBase
{
    private readonly IAuthService _authService;

    public AuthController(IAuthService authService)
    {
        _authService = authService;
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginRequest request)
    {
        var response = await _authService.LoginAsync(request);
        return Ok(ApiResponse<LoginResponse>.Ok(response, "Login successful."));
    }

    [HttpPost("register-admin")]
    public async Task<IActionResult> RegisterAdmin([FromBody] AdminRegisterRequest request)
    {
        var response = await _authService.RegisterAdminAsync(request);
        return Ok(ApiResponse<LoginResponse>.Ok(response, "Admin registered successfully."));
    }
}
