using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using School.Application.DTOs.Auth;
using School.Application.DTOs.Common;
using School.Application.Features.Auth.Commands;

namespace School.Api.Controllers;

[ApiController]
[Route("api/auth")]
public class AuthController : ControllerBase
{
    private readonly IMediator _mediator;

    public AuthController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost("register-admin")]
    [AllowAnonymous]
    public async Task<IActionResult> RegisterAdmin([FromBody] RegisterAdminCommand command)
    {
        var response = await _mediator.Send(command);
        return Ok(ApiResponse<AuthResponse>.Ok(response, "Admin registered successfully."));
    }
}
