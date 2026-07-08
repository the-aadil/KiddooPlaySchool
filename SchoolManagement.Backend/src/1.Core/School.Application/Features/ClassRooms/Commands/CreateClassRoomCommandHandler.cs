using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using School.Application.DTOs.ClassRooms;
using School.Application.Interfaces;
using School.Domain.Entities;
using School.Domain.Exceptions;

namespace School.Application.Features.ClassRooms.Commands;

public class CreateClassRoomCommandHandler : IRequestHandler<CreateClassRoomCommand, ClassRoomResponse>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public CreateClassRoomCommandHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<ClassRoomResponse> Handle(
        CreateClassRoomCommand request, CancellationToken cancellationToken)
    {
        if (request.Capacity <= 0)
        {
            throw new DomainException("Capacity must be greater than zero.");
        }

        var exists = await _context.ClassRooms
            .AnyAsync(c => c.Name == request.Name && !c.IsDeleted, cancellationToken);

        if (exists)
        {
            throw new DomainException($"A classroom with name '{request.Name}' already exists.");
        }

        var classRoom = new ClassRoom
        {
            Name = request.Name,
            Description = request.Description,
            Capacity = request.Capacity,
            AgeGroup = request.AgeGroup,
            IsActive = true
        };

        _context.ClassRooms.Add(classRoom);
        await _context.SaveChangesAsync(cancellationToken);

        return _mapper.Map<ClassRoomResponse>(classRoom);
    }
}
