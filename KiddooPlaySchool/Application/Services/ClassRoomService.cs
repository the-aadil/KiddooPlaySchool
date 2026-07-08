using AutoMapper;
using KiddooPlaySchool.Application.DTOs.ClassRoom;
using KiddooPlaySchool.Application.Interfaces;
using KiddooPlaySchool.Domain.Entities;
using KiddooPlaySchool.Domain.Exceptions;
using KiddooPlaySchool.Domain.Interfaces;
using KiddooPlaySchool.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace KiddooPlaySchool.Application.Services;

public class ClassRoomService : IClassRoomService
{
    private readonly IRepository<ClassRoom> _classRoomRepository;
    private readonly IRepository<TeacherProfile> _teacherRepository;
    private readonly AppDbContext _context;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public ClassRoomService(
        IRepository<ClassRoom> classRoomRepository,
        IRepository<TeacherProfile> teacherRepository,
        AppDbContext context,
        IUnitOfWork unitOfWork,
        IMapper mapper)
    {
        _classRoomRepository = classRoomRepository;
        _teacherRepository = teacherRepository;
        _context = context;
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
            Capacity = request.Capacity,
            AgeGroup = request.AgeGroup,
            IsActive = true
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
            .Include(c => c.TeacherClassRooms)
                .ThenInclude(tc => tc.TeacherProfile)
                    .ThenInclude(t => t.User)
            .FirstOrDefaultAsync(c => c.Id == id && !c.IsDeleted)
            ?? throw new EntityNotFoundException(nameof(ClassRoom), id);

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
            ?? throw new EntityNotFoundException(nameof(ClassRoom), id);

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
            ?? throw new EntityNotFoundException(nameof(ClassRoom), id);

        await _classRoomRepository.DeleteAsync(classRoom);
        await _unitOfWork.SaveChangesAsync();
    }

    public async Task AssignTeacherAsync(AssignTeacherRequest request)
    {
        var classRoom = await _classRoomRepository.GetAll()
            .FirstOrDefaultAsync(c => c.Id == request.ClassRoomId && !c.IsDeleted)
            ?? throw new EntityNotFoundException(nameof(ClassRoom), request.ClassRoomId);

        var teacher = await _teacherRepository.GetAll()
            .FirstOrDefaultAsync(t => t.Id == request.TeacherProfileId && !t.IsDeleted)
            ?? throw new EntityNotFoundException(nameof(TeacherProfile), request.TeacherProfileId);

        var alreadyAssigned = await _context.TeacherClassRooms
            .AnyAsync(tc => tc.TeacherProfileId == request.TeacherProfileId
                         && tc.ClassRoomId == request.ClassRoomId);

        if (alreadyAssigned)
        {
            throw new InvalidOperationException("Teacher is already assigned to this classroom.");
        }

        _context.TeacherClassRooms.Add(new TeacherClassRoom
        {
            TeacherProfileId = request.TeacherProfileId,
            ClassRoomId = request.ClassRoomId,
            AssignmentDate = DateTime.UtcNow
        });

        await _unitOfWork.SaveChangesAsync();
    }

    public async Task RemoveTeacherAssignmentAsync(Guid teacherProfileId, Guid classRoomId)
    {
        var assignment = await _context.TeacherClassRooms
            .FirstOrDefaultAsync(tc => tc.TeacherProfileId == teacherProfileId
                                    && tc.ClassRoomId == classRoomId)
            ?? throw new EntityNotFoundException("TeacherClassRoom assignment not found.");

        _context.TeacherClassRooms.Remove(assignment);
        await _unitOfWork.SaveChangesAsync();
    }

    public async Task<IEnumerable<ClassRoomResponse>> GetTeacherClassRoomsAsync(Guid teacherProfileId)
    {
        var classRoomIds = await _context.TeacherClassRooms
            .Where(tc => tc.TeacherProfileId == teacherProfileId)
            .Select(tc => tc.ClassRoomId)
            .ToListAsync();

        var classRooms = await _classRoomRepository.GetAll()
            .Include(c => c.Students)
            .Where(c => classRoomIds.Contains(c.Id) && !c.IsDeleted)
            .ToListAsync();

        return _mapper.Map<IEnumerable<ClassRoomResponse>>(classRooms);
    }
}
