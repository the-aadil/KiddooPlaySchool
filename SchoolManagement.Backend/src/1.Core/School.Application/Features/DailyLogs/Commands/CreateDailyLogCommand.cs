using MediatR;
using School.Application.DTOs.DailyLogs;
using School.Domain.Enums;

namespace School.Application.Features.DailyLogs.Commands;

public class CreateDailyLogCommand : IRequest<DailyLogResponse>
{
    public Guid StudentId { get; set; }
    public DateTimeOffset LogTimestamp { get; set; }
    public ActivityType ActivityType { get; set; }
    public string? Payload { get; set; }
    public ActivityVisibility Visibility { get; set; }
    public string? Remarks { get; set; }
    public List<string> MediaUrls { get; set; } = [];
}
