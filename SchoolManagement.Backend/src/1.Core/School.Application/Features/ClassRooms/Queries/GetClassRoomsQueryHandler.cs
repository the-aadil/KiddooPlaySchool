using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using School.Application.DTOs.ClassRooms;
using School.Application.DTOs.Common;
using School.Application.Interfaces;

namespace School.Application.Features.ClassRooms.Queries;

public class GetClassRoomsQueryHandler : IRequestHandler<GetClassRoomsQuery, PagedResponse<ClassRoomResponse>>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public GetClassRoomsQueryHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<PagedResponse<ClassRoomResponse>> Handle(
        GetClassRoomsQuery request, CancellationToken cancellationToken)
    {
        var query = _context.ClassRooms
            .AsNoTracking()
            .Include(c => c.Students)
            .Where(c => !c.IsDeleted);

        if (request.IsActive.HasValue)
        {
            query = query.Where(c => c.IsActive == request.IsActive.Value);
        }

        var totalCount = await query.CountAsync(cancellationToken);

        var classRooms = await query
            .OrderBy(c => c.Name)
            .Skip((request.PageNumber - 1) * request.PageSize)
            .Take(request.PageSize)
            .ToListAsync(cancellationToken);

        var items = _mapper.Map<List<ClassRoomResponse>>(classRooms);

        return new PagedResponse<ClassRoomResponse>(items, totalCount, request.PageNumber, request.PageSize);
    }
}
