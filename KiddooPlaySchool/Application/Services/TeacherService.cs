using AutoMapper;
using KiddooPlaySchool.Application.DTOs.Teacher;
using KiddooPlaySchool.Application.Interfaces;
using KiddooPlaySchool.Domain.Entities;
using KiddooPlaySchool.Domain.Enums;
using KiddooPlaySchool.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace KiddooPlaySchool.Application.Services;

public class TeacherService : ITeacherService
{
    private readonly IRepository<ApplicationUser> _userRepository;
    private readonly IRepository<TeacherProfile> _teacherProfileRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public TeacherService(
        IRepository<ApplicationUser> userRepository,
        IRepository<TeacherProfile> teacherProfileRepository,
        IUnitOfWork unitOfWork,
        IMapper mapper)
    {
        _userRepository = userRepository;
        _teacherProfileRepository = teacherProfileRepository;
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<TeacherResponse> CreateAsync(CreateTeacherRequest request)
    {
        if (request.Password != request.ConfirmPassword)
        {
            throw new ArgumentException("Password and confirmation password do not match.");
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
            Role = UserRole.Teacher,
            IsActive = true
        };

        await _userRepository.AddAsync(user);

        var profile = new TeacherProfile
        {
            UserId = user.Id,
            AlternateMobile = request.AlternateMobile,
            DateOfBirth = request.DateOfBirth,
            Gender = request.Gender,
            Qualification = request.Qualification,
            Specialization = request.Specialization,
            Address = request.Address,
            City = request.City,
            State = request.State,
            JoinDate = request.JoinDate == default ? DateTime.UtcNow : request.JoinDate,
            EmployeeId = await GenerateEmployeeIdAsync()
        };

        await _teacherProfileRepository.AddAsync(profile);
        await _unitOfWork.SaveChangesAsync();

        return _mapper.Map<TeacherResponse>(profile);
    }

    public async Task<TeacherResponse> GetByIdAsync(Guid id)
    {
        var profile = await _teacherProfileRepository.GetAll()
            .Include(p => p.User)
            .FirstOrDefaultAsync(p => p.Id == id && !p.IsDeleted)
            ?? throw new KeyNotFoundException($"Teacher profile with ID {id} not found.");

        return _mapper.Map<TeacherResponse>(profile);
    }

    public async Task<IEnumerable<TeacherResponse>> GetAllAsync()
    {
        var profiles = await _teacherProfileRepository.GetAll()
            .Include(p => p.User)
            .Where(p => !p.IsDeleted)
            .ToListAsync();

        return _mapper.Map<IEnumerable<TeacherResponse>>(profiles);
    }

    public async Task UpdateAsync(Guid id, UpdateTeacherRequest request)
    {
        var profile = await _teacherProfileRepository.GetAll()
            .Include(p => p.User)
            .FirstOrDefaultAsync(p => p.Id == id && !p.IsDeleted)
            ?? throw new KeyNotFoundException($"Teacher profile with ID {id} not found.");

        var user = profile.User;
        user.FirstName = request.FirstName;
        user.LastName = request.LastName;
        user.MobileNumber = request.MobileNumber;

        if (!string.IsNullOrWhiteSpace(request.Password))
        {
            user.PasswordHash = BCrypt.Net.BCrypt.HashPassword(request.Password);
        }

        profile.AlternateMobile = request.AlternateMobile;
        profile.DateOfBirth = request.DateOfBirth;
        profile.Gender = request.Gender;
        profile.Qualification = request.Qualification;
        profile.Specialization = request.Specialization;
        profile.Address = request.Address;
        profile.City = request.City;
        profile.State = request.State;

        await _unitOfWork.SaveChangesAsync();
    }

    public async Task DeleteAsync(Guid id)
    {
        var profile = await _teacherProfileRepository.GetAll()
            .FirstOrDefaultAsync(p => p.Id == id && !p.IsDeleted)
            ?? throw new KeyNotFoundException($"Teacher profile with ID {id} not found.");

        await _teacherProfileRepository.DeleteAsync(profile);

        var user = await _userRepository.GetAll()
            .FirstOrDefaultAsync(u => u.Id == profile.UserId);

        if (user is not null)
        {
            await _userRepository.DeleteAsync(user);
        }

        await _unitOfWork.SaveChangesAsync();
    }

    private async Task<string> GenerateEmployeeIdAsync()
    {
        var year = DateTime.UtcNow.Year;
        var existingIds = await _teacherProfileRepository.GetAll()
            .Where(p => !p.IsDeleted)
            .Select(p => p.EmployeeId)
            .ToHashSetAsync();

        for (var attempt = 0; attempt < 5; attempt++)
        {
            var number = Random.Shared.Next(10000, 99999);
            var employeeId = $"TCH{year}{number}";

            if (!existingIds.Contains(employeeId))
            {
                return employeeId;
            }
        }

        var finalNumber = Random.Shared.Next(100000, 999999);
        return $"TCH{year}{finalNumber}";
    }
}
