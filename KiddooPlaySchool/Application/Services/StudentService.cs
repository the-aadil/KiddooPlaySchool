using AutoMapper;
using KiddooPlaySchool.Application.DTOs.Student;
using KiddooPlaySchool.Application.Interfaces;
using KiddooPlaySchool.Domain.Entities;
using KiddooPlaySchool.Domain.Enums;
using KiddooPlaySchool.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace KiddooPlaySchool.Application.Services;

public class StudentService : IStudentService
{
    private readonly IRepository<ApplicationUser> _userRepository;
    private readonly IRepository<StudentProfile> _studentProfileRepository;
    private readonly IRepository<ClassRoom> _classRoomRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public StudentService(
        IRepository<ApplicationUser> userRepository,
        IRepository<StudentProfile> studentProfileRepository,
        IRepository<ClassRoom> classRoomRepository,
        IUnitOfWork unitOfWork,
        IMapper mapper)
    {
        _userRepository = userRepository;
        _studentProfileRepository = studentProfileRepository;
        _classRoomRepository = classRoomRepository;
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<StudentResponse> CreateAsync(CreateStudentRequest request)
    {
        if (request.Password != request.ConfirmPassword)
        {
            throw new ArgumentException("Password and confirmation password do not match.");
        }

        var users = _userRepository.GetAll();

        if (await users.AnyAsync(u => u.Email == request.Email))
        {
            throw new InvalidOperationException("Email is already registered.");
        }

        if (request.ClassRoomId.HasValue)
        {
            var classroomExists = await _classRoomRepository.GetAll()
                .AnyAsync(c => c.Id == request.ClassRoomId && !c.IsDeleted);

            if (!classroomExists)
            {
                throw new ArgumentException($"ClassRoom with ID {request.ClassRoomId} does not exist.");
            }
        }

        var username = request.Email.Split('@')[0];

        if (await users.AnyAsync(u => u.Username == username))
        {
            username = $"{username}{Guid.CreateVersion7().ToString()[..4]}";
        }

        var user = new ApplicationUser
        {
            Username = username,
            Email = request.Email,
            PasswordHash = BCrypt.Net.BCrypt.HashPassword(request.Password),
            FirstName = request.FirstName,
            LastName = request.LastName,
            MobileNumber = request.Phone,
            Role = UserRole.Student,
            IsActive = true
        };

        await _userRepository.AddAsync(user);

        var profile = new StudentProfile
        {
            UserId = user.Id,
            DateOfBirth = request.DateOfBirth,
            Address = request.Address,
            ParentName = request.ParentName,
            ParentPhone = request.ParentPhone,
            ParentEmail = request.ParentEmail,
            EnrollmentDate = DateTime.UtcNow,
            ClassRoomId = request.ClassRoomId
        };

        await _studentProfileRepository.AddAsync(profile);
        await _unitOfWork.SaveChangesAsync();

        return _mapper.Map<StudentResponse>(profile);
    }

    public async Task<StudentResponse> GetByIdAsync(Guid id)
    {
        var profile = await _studentProfileRepository.GetAll()
            .Include(p => p.User)
            .Include(p => p.ClassRoom)
            .FirstOrDefaultAsync(p => p.Id == id && !p.IsDeleted)
            ?? throw new KeyNotFoundException($"Student profile with ID {id} not found.");

        return _mapper.Map<StudentResponse>(profile);
    }

    public async Task<IEnumerable<StudentResponse>> GetAllAsync()
    {
        var profiles = await _studentProfileRepository.GetAll()
            .Include(p => p.User)
            .Include(p => p.ClassRoom)
            .Where(p => !p.IsDeleted)
            .ToListAsync();

        return _mapper.Map<IEnumerable<StudentResponse>>(profiles);
    }

    public async Task<IEnumerable<StudentResponse>> GetByClassRoomAsync(Guid classRoomId)
    {
        var profiles = await _studentProfileRepository.GetAll()
            .Include(p => p.User)
            .Include(p => p.ClassRoom)
            .Where(p => p.ClassRoomId == classRoomId && !p.IsDeleted)
            .ToListAsync();

        return _mapper.Map<IEnumerable<StudentResponse>>(profiles);
    }

    public async Task UpdateAsync(Guid id, UpdateStudentRequest request)
    {
        var profile = await _studentProfileRepository.GetAll()
            .Include(p => p.User)
            .FirstOrDefaultAsync(p => p.Id == id && !p.IsDeleted)
            ?? throw new KeyNotFoundException($"Student profile with ID {id} not found.");

        if (request.ClassRoomId.HasValue)
        {
            var classroomExists = await _classRoomRepository.GetAll()
                .AnyAsync(c => c.Id == request.ClassRoomId && !c.IsDeleted);

            if (!classroomExists)
            {
                throw new ArgumentException($"ClassRoom with ID {request.ClassRoomId} does not exist.");
            }
        }

        var emailTaken = await _userRepository.GetAll()
            .AnyAsync(u => u.Email == request.Email && u.Id != profile.UserId && !u.IsDeleted);

        if (emailTaken)
        {
            throw new InvalidOperationException("Email is already registered to another user.");
        }

        var user = profile.User;
        user.FirstName = request.FirstName;
        user.LastName = request.LastName;
        user.Email = request.Email;
        user.MobileNumber = request.Phone;

        if (!string.IsNullOrWhiteSpace(request.Password))
        {
            user.PasswordHash = BCrypt.Net.BCrypt.HashPassword(request.Password);
        }

        profile.DateOfBirth = request.DateOfBirth;
        profile.Address = request.Address;
        profile.ParentName = request.ParentName;
        profile.ParentPhone = request.ParentPhone;
        profile.ParentEmail = request.ParentEmail;
        profile.ClassRoomId = request.ClassRoomId;

        await _unitOfWork.SaveChangesAsync();
    }

    public async Task DeleteAsync(Guid id)
    {
        var profile = await _studentProfileRepository.GetAll()
            .FirstOrDefaultAsync(p => p.Id == id && !p.IsDeleted)
            ?? throw new KeyNotFoundException($"Student profile with ID {id} not found.");

        await _studentProfileRepository.DeleteAsync(profile);

        var user = await _userRepository.GetAll()
            .FirstOrDefaultAsync(u => u.Id == profile.UserId);

        if (user is not null)
        {
            await _userRepository.DeleteAsync(user);
        }

        await _unitOfWork.SaveChangesAsync();
    }
}
