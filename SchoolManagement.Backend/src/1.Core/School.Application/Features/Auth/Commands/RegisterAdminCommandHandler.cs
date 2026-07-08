using MediatR;
using Microsoft.EntityFrameworkCore;
using School.Application.DTOs.Auth;
using School.Application.Interfaces;
using School.Domain.Entities;
using School.Domain.Enums;
using School.Domain.Exceptions;

namespace School.Application.Features.Auth.Commands;

public class RegisterAdminCommandHandler : IRequestHandler<RegisterAdminCommand, AuthResponse>
{
    private readonly IApplicationDbContext _context;
    private readonly IJwtService _jwtService;

    public RegisterAdminCommandHandler(IApplicationDbContext context, IJwtService jwtService)
    {
        _context = context;
        _jwtService = jwtService;
    }

    public async Task<AuthResponse> Handle(RegisterAdminCommand request, CancellationToken cancellationToken)
    {
        var usernameTaken = await _context.ApplicationUsers
            .AnyAsync(u => u.Username == request.Username && !u.IsDeleted, cancellationToken);

        if (usernameTaken)
            throw new DomainException($"Username '{request.Username}' is already taken.");

        var emailTaken = await _context.ApplicationUsers
            .AnyAsync(u => u.Email == request.Email && !u.IsDeleted, cancellationToken);

        if (emailTaken)
            throw new DomainException($"Email '{request.Email}' is already registered.");

        var user = new ApplicationUser
        {
            Username = request.Username,
            Email = request.Email,
            PasswordHash = BCrypt.Net.BCrypt.HashPassword(request.Password),
            FirstName = request.FirstName,
            LastName = request.LastName,
            MobileNumber = request.MobileNumber,
            Role = UserRole.Admin,
            IsActive = true
        };

        _context.ApplicationUsers.Add(user);
        await _context.SaveChangesAsync(cancellationToken);

        var adminProfile = new AdminProfile
        {
            UserId = user.Id,
            Department = request.Department,
            Designation = request.Designation
        };

        _context.AdminProfiles.Add(adminProfile);
        await _context.SaveChangesAsync(cancellationToken);

        var token = _jwtService.GenerateToken(user.Id, user.Username, UserRole.Admin);

        return new AuthResponse
        {
            UserId = user.Id,
            Username = user.Username,
            Email = user.Email,
            FirstName = user.FirstName,
            LastName = user.LastName,
            Role = UserRole.Admin,
            Token = token
        };
    }
}
