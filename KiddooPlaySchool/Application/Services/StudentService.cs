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

    public async Task CreateAsync(StudentDto student)
    {
        var entity = _mapper.Map<Student>(student);
        await _repository.AddAsync(entity);
        await _unitOfWork.SaveChangesAsync();
    }

    public async Task UpdateAsync(StudentDto student)
    {
        var entity = await _repository.GetByIdAsync(student.Id);
        if (entity is null) throw new KeyNotFoundException($"Student with Id {student.Id} not found.");

        _mapper.Map(student, entity);
        await _repository.UpdateAsync(entity);
        await _unitOfWork.SaveChangesAsync();
    }

    public async Task DeleteAsync(Guid id)
    {
        var entity = await _repository.GetByIdAsync(id);
        if (entity is null) throw new KeyNotFoundException($"Student with Id {id} not found.");

        await _repository.DeleteAsync(entity);
        await _unitOfWork.SaveChangesAsync();
    }
}
