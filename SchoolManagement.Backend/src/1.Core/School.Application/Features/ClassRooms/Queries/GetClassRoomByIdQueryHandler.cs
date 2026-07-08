using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using School.Application.DTOs.ClassRooms;
using School.Application.Interfaces;
using School.Domain.Exceptions;

namespace School.Application.Features.ClassRooms.Queries;

public class GetClassRoomByIdQueryHandler : IRequestHandler<GetClassRoomByIdQuery, ClassRoomResponse>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public GetClassRoomByIdQueryHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<ClassRoomResponse> Handle(
        GetClassRoomByIdQuery request, CancellationToken cancellationToken)
    {
        var classRoom = await _context.ClassRooms
            .AsNoTracking()
            .Include(c => c.Students)
            .FirstOrDefaultAsync(c => c.Id == request.Id && !c.IsDeleted, cancellationToken)
            ?? throw new DomainException($"ClassRoom with ID {request.Id} was not found.");

        return _mapper.Map<ClassRoomResponse>(classRoom);
    }
}
