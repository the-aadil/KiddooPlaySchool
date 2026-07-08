using FluentValidation;

namespace School.Application.Features.DailyLogs.Queries;

public class GetStudentDailyLogsQueryValidator : AbstractValidator<GetStudentDailyLogsQuery>
{
    public GetStudentDailyLogsQueryValidator()
    {
        RuleFor(x => x.StudentId)
            .NotEmpty().WithMessage("Student ID is required.");

        RuleFor(x => x.PageNumber)
            .GreaterThanOrEqualTo(1).WithMessage("Page number must be at least 1.");

        RuleFor(x => x.PageSize)
            .InclusiveBetween(1, 100).WithMessage("Page size must be between 1 and 100.");
    }
}
