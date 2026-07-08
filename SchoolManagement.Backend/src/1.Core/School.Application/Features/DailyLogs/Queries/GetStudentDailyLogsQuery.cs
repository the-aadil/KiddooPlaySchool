using MediatR;
using School.Application.DTOs.Common;
using School.Application.DTOs.DailyLogs;

namespace School.Application.Features.DailyLogs.Queries;

public class GetStudentDailyLogsQuery : IRequest<PagedResponse<DailyLogResponse>>
{
    public Guid StudentId { get; set; }
    public int PageNumber { get; set; } = 1;
    public int PageSize { get; set; } = 20;
    public DateOnly? FromDate { get; set; }
    public DateOnly? ToDate { get; set; }
}
