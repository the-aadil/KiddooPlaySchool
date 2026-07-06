using AutoMapper;
using KiddooPlaySchool.Application.DTOs.ClassRoom;
using KiddooPlaySchool.Application.Interfaces;
using KiddooPlaySchool.Domain.Entities;
using KiddooPlaySchool.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace KiddooPlaySchool.Application.Services;

public class ClassRoomService : IClassRoomService
{
    private readonly IRepository<ClassRoom> _classRoomRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public ClassRoomService(
        IRepository<ClassRoom> classRoomRepository,
        IUnitOfWork unitOfWork,
        IMapper mapper)
    {
        _classRoomRepository = classRoomRepository;
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<ClassRoomResponse> CreateAsync(CreateClassRoomRequest request)
    {
        if (request.Capacity <= 0)
        {
            throw new ArgumentException("Capacity must be greater than zero.");
        }

        if (await _classRoomRepository.GetAll().AnyAsync(c => c.Name == request.Name && !c.IsDeleted))
        {
            throw new InvalidOperationException($"A classroom with name '{request.Name}' already exists.");
        }

        var classRoom = new ClassRoom
        {
            Name = request.Name,
            Description = request.Description,
            Capacity = request.Capacity
        };

        await _classRoomRepository.AddAsync(classRoom);
        await _unitOfWork.SaveChangesAsync();

        var response = _mapper.Map<ClassRoomResponse>(classRoom);
        response.StudentCount = 0;

        return response;
    }

    public async Task<ClassRoomResponse> GetByIdAsync(Guid id)
    {
        var classRoom = await _classRoomRepository.GetAll()
            .Include(c => c.Students)
            .FirstOrDefaultAsync(c => c.Id == id && !c.IsDeleted)
            ?? throw new KeyNotFoundException($"ClassRoom with ID {id} not found.");

        return _mapper.Map<ClassRoomResponse>(classRoom);
    }

    public async Task<IEnumerable<ClassRoomResponse>> GetAllAsync()
    {
        var classRooms = await _classRoomRepository.GetAll()
            .Include(c => c.Students)
            .Where(c => !c.IsDeleted)
            .ToListAsync();

        return _mapper.Map<IEnumerable<ClassRoomResponse>>(classRooms);
    }

    public async Task UpdateAsync(Guid id, UpdateClassRoomRequest request)
    {
        if (request.Capacity <= 0)
        {
            throw new ArgumentException("Capacity must be greater than zero.");
        }

        var classRoom = await _classRoomRepository.GetAll()
            .FirstOrDefaultAsync(c => c.Id == id && !c.IsDeleted)
            ?? throw new KeyNotFoundException($"ClassRoom with ID {id} not found.");

        var duplicate = await _classRoomRepository.GetAll()
            .AnyAsync(c => c.Name == request.Name && c.Id != id && !c.IsDeleted);

        if (duplicate)
        {
            throw new InvalidOperationException($"A classroom with name '{request.Name}' already exists.");
        }

        classRoom.Name = request.Name;
        classRoom.Description = request.Description;
        classRoom.Capacity = request.Capacity;

        await _unitOfWork.SaveChangesAsync();
    }

    public async Task DeleteAsync(Guid id)
    {
        var classRoom = await _classRoomRepository.GetAll()
            .FirstOrDefaultAsync(c => c.Id == id && !c.IsDeleted)
            ?? throw new KeyNotFoundException($"ClassRoom with ID {id} not found.");

        await _classRoomRepository.DeleteAsync(classRoom);
        await _unitOfWork.SaveChangesAsync();
    }
}
