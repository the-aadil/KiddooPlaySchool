using AutoMapper;
using KiddooPlaySchool.Application.DTOs;
using KiddooPlaySchool.Application.Interfaces;
using KiddooPlaySchool.Domain.Entities;
using KiddooPlaySchool.Domain.Interfaces;

namespace KiddooPlaySchool.Application.Services;

public class TeacherService : ITeacherService
{
    private readonly IRepository<Teacher> _repository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public TeacherService(IRepository<Teacher> repository, IUnitOfWork unitOfWork, IMapper mapper)
    {
        _repository = repository;
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<TeacherResponse> RegisterAsync(RegisterTeacherRequest request)
    {
        var all = await _repository.GetAllAsync();

        if (all.Any(t => t.Username.Equals(request.Username, StringComparison.OrdinalIgnoreCase)))
            throw new InvalidOperationException("Username already exists");

        if (all.Any(t => t.Email.Equals(request.Email, StringComparison.OrdinalIgnoreCase)))
            throw new InvalidOperationException("Email already registered");

        if (all.Any(t => t.MobileNumber == request.MobileNumber))
            throw new InvalidOperationException("Mobile number already registered");

        var teacher = _mapper.Map<Teacher>(request);
        teacher.PasswordHash = BCrypt.Net.BCrypt.HashPassword(request.Password);
        teacher.EmployeeId = $"TCH-{DateTime.UtcNow:yyyyMMdd}-{Guid.NewGuid().ToString()[..4].ToUpper()}";
        teacher.JoinDate = DateTime.UtcNow;
        teacher.IsActive = true;

        await _repository.AddAsync(teacher);
        await _unitOfWork.SaveChangesAsync();

        return _mapper.Map<TeacherResponse>(teacher);
    }

    public async Task<TeacherResponse?> GetByIdAsync(Guid id)
    {
        var teacher = await _repository.GetByIdAsync(id);
        return teacher is null ? null : _mapper.Map<TeacherResponse>(teacher);
    }

    public async Task<IEnumerable<TeacherResponse>> GetAllAsync()
    {
        var teachers = await _repository.GetAllAsync();
        return _mapper.Map<IEnumerable<TeacherResponse>>(teachers);
    }

    public async Task DeleteAsync(Guid id)
    {
        var teacher = await _repository.GetByIdAsync(id)
            ?? throw new KeyNotFoundException("Teacher not found");

        await _repository.DeleteAsync(teacher);
        await _unitOfWork.SaveChangesAsync();
    }
}
