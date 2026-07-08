using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using School.Application.DTOs.Common;
using School.Application.DTOs.DailyLogs;
using School.Application.Interfaces;
using School.Domain.Enums;

namespace School.Application.Features.DailyLogs.Queries;

public class GetStudentDailyLogsQueryHandler
    : IRequestHandler<GetStudentDailyLogsQuery, PagedResponse<DailyLogResponse>>
{
    private readonly IApplicationDbContext _context;
    private readonly ICurrentUserContext _currentUser;
    private readonly IMapper _mapper;

    public GetStudentDailyLogsQueryHandler(
        IApplicationDbContext context,
        ICurrentUserContext currentUser,
        IMapper mapper)
    {
        _context = context;
        _currentUser = currentUser;
        _mapper = mapper;
    }

    public async Task<PagedResponse<DailyLogResponse>> Handle(
        GetStudentDailyLogsQuery request, CancellationToken cancellationToken)
    {
        var query = _context.DailyActivityLogs
            .AsNoTracking()
            .Include(d => d.Student).ThenInclude(s => s.User)
            .Include(d => d.Teacher).ThenInclude(t => t.User)
            .Where(d => d.StudentId == request.StudentId && !d.IsDeleted);

        if (_currentUser.Role == UserRole.Student)
        {
            query = query.Where(d => d.Visibility == ActivityVisibility.SharedWithParent);
        }

        if (request.FromDate.HasValue)
        {
            query = query.Where(d =>
                DateOnly.FromDateTime(d.LogTimestamp.DateTime) >= request.FromDate.Value);
        }

        if (request.ToDate.HasValue)
        {
            query = query.Where(d =>
                DateOnly.FromDateTime(d.LogTimestamp.DateTime) <= request.ToDate.Value);
        }

        var totalCount = await query.CountAsync(cancellationToken);

        var logs = await query
            .OrderByDescending(d => d.LogTimestamp)
            .Skip((request.PageNumber - 1) * request.PageSize)
            .Take(request.PageSize)
            .ToListAsync(cancellationToken);

        var items = _mapper.Map<List<DailyLogResponse>>(logs);

        return new PagedResponse<DailyLogResponse>(items, totalCount, request.PageNumber, request.PageSize);
    }
}
