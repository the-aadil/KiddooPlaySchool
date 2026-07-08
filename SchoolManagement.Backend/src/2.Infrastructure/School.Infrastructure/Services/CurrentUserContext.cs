using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using School.Application.Interfaces;

namespace School.Infrastructure.Services;

public class CurrentUserContext : ICurrentUserContext
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public CurrentUserContext(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    public Guid UserId
    {
        get
        {
            var value = _httpContextAccessor.HttpContext?.User
                .FindFirst(ClaimTypes.NameIdentifier)?.Value;
            return value is not null ? Guid.Parse(value) : Guid.Empty;
        }
    }

    public string UserName
    {
        get
        {
            return _httpContextAccessor.HttpContext?.User
                .FindFirst(ClaimTypes.Name)?.Value ?? "System";
        }
    }

    public string Role
    {
        get
        {
            return _httpContextAccessor.HttpContext?.User
                .FindFirst(ClaimTypes.Role)?.Value ?? string.Empty;
        }
    }

    public bool IsAuthenticated =>
        _httpContextAccessor.HttpContext?.User.Identity?.IsAuthenticated ?? false;
}
