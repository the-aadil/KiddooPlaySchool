using KiddooPlaySchool.Application.DTOs.Auth;
using KiddooPlaySchool.Application.Interfaces;
using KiddooPlaySchool.Domain.Entities;
using KiddooPlaySchool.Domain.Enums;
using KiddooPlaySchool.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace KiddooPlaySchool.Application.Services;

public class AuthService : IAuthService
{
    private readonly IRepository<ApplicationUser> _userRepository;
    private readonly IRepository<AdminProfile> _adminProfileRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IJwtTokenGenerator _tokenGenerator;

    public AuthService(
        IRepository<ApplicationUser> userRepository,
        IRepository<AdminProfile> adminProfileRepository,
        IUnitOfWork unitOfWork,
        IJwtTokenGenerator tokenGenerator)
    {
        _userRepository = userRepository;
        _adminProfileRepository = adminProfileRepository;
        _unitOfWork = unitOfWork;
        _tokenGenerator = tokenGenerator;
    }

    public async Task<LoginResponse> LoginAsync(LoginRequest request)
    {
        var user = await _userRepository.GetAll()
            .FirstOrDefaultAsync(u => u.Username == request.Username && !u.IsDeleted);

        if (user is null || !BCrypt.Net.BCrypt.Verify(request.Password, user.PasswordHash))
        {
            throw new UnauthorizedAccessException("Invalid username or password.");
        }

        if (!user.IsActive)
        {
            throw new UnauthorizedAccessException("Account is deactivated. Contact the administrator.");
        }

        var token = _tokenGenerator.GenerateToken(user);

        return new LoginResponse
        {
            Token = token,
            Role = user.Role,
            FullName = $"{user.FirstName} {user.LastName}",
            UserId = user.Id
        };
    }

    public async Task<LoginResponse> RegisterAdminAsync(AdminRegisterRequest request)
    {
        if (request.Password != request.ConfirmPassword)
        {
            throw new ArgumentException("Password and confirmation password do not match.");
        }

        var anyAdminExists = await _userRepository.GetAll()
            .AnyAsync(u => u.Role == UserRole.Admin && !u.IsDeleted);

        if (anyAdminExists)
        {
            throw new InvalidOperationException("An admin already exists. Only the initial setup can register an admin.");
        }

        var users = _userRepository.GetAll();

        if (await users.AnyAsync(u => u.Username == request.Username))
        {
            throw new InvalidOperationException("Username is already taken.");
        }

        if (await users.AnyAsync(u => u.Email == request.Email))
        {
            throw new InvalidOperationException("Email is already registered.");
        }

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

        await _userRepository.AddAsync(user);

        var profile = new AdminProfile
        {
            UserId = user.Id,
            Department = "Administration",
            Designation = "System Administrator"
        };

        await _adminProfileRepository.AddAsync(profile);
        await _unitOfWork.SaveChangesAsync();

        var token = _tokenGenerator.GenerateToken(user);

        return new LoginResponse
        {
            Token = token,
            Role = user.Role,
            FullName = $"{user.FirstName} {user.LastName}",
            UserId = user.Id
        };
    }
}
