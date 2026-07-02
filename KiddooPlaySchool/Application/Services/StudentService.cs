using AutoMapper;
using KiddooPlaySchool.Application.DTOs;
using KiddooPlaySchool.Application.Interfaces;
using KiddooPlaySchool.Domain.Entities;
using KiddooPlaySchool.Domain.Interfaces;

namespace KiddooPlaySchool.Application.Services;

public class StudentService : IStudentService
{
    private readonly IRepository<Student> _repository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public StudentService(IRepository<Student> repository, IUnitOfWork unitOfWork, IMapper mapper)
    {
        _repository = repository;
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<IEnumerable<StudentDto>> GetAllAsync()
    {
        var students = await _repository.GetAllAsync();
        return _mapper.Map<IEnumerable<StudentDto>>(students);
    }

    public async Task<StudentDto?> GetByIdAsync(Guid id)
    {
        var student = await _repository.GetByIdAsync(id);
        return student is null ? null : _mapper.Map<StudentDto>(student);
    }

    public async Task<Guid> CreateAsync(CreateStudentRequest request)
    {
        var entity = new Student
        {
            FirstName = request.FirstName,
            LastName = request.LastName,
            Email = request.Email,
            Phone = request.Phone,
            DateOfBirth = request.DateOfBirth,
            Address = request.Address,
            ParentName = request.ParentName,
            ParentPhone = request.ParentPhone,
            ParentEmail = request.ParentEmail
        };

        await _repository.AddAsync(entity);
        await _unitOfWork.SaveChangesAsync();
        return entity.Id;
    }

    public async Task UpdateAsync(StudentDto student)
    {
        var entity = await _repository.GetByIdAsync(student.Id)
            ?? throw new KeyNotFoundException($"Student with Id {student.Id} not found.");

        entity.Email = student.Email;
        await _repository.UpdateAsync(entity);
        await _unitOfWork.SaveChangesAsync();
    }

    public async Task DeleteAsync(Guid id)
    {
        var entity = await _repository.GetByIdAsync(id)
            ?? throw new KeyNotFoundException($"Student with Id {id} not found.");

        await _repository.DeleteAsync(entity);
        await _unitOfWork.SaveChangesAsync();
    }
}
