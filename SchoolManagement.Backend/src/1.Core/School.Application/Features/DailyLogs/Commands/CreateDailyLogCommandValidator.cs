using FluentValidation;
using School.Domain.Enums;

namespace School.Application.Features.DailyLogs.Commands;

public class CreateDailyLogCommandValidator : AbstractValidator<CreateDailyLogCommand>
{
    public CreateDailyLogCommandValidator()
    {
        RuleFor(x => x.StudentId)
            .NotEmpty().WithMessage("Student ID is required.");

        RuleFor(x => x.ActivityType)
            .IsInEnum().WithMessage("Invalid activity type.");

        RuleFor(x => x.LogTimestamp)
            .NotEmpty().WithMessage("Log timestamp is required.")
            .LessThanOrEqualTo(DateTimeOffset.UtcNow.AddHours(1))
            .WithMessage("Daily activity log cannot be recorded for future dates.");

        RuleFor(x => x.Visibility)
            .IsInEnum().WithMessage("Invalid visibility setting.");

        RuleFor(x => x.Payload)
            .MaximumLength(2000).WithMessage("Payload must not exceed 2000 characters.");

        RuleFor(x => x.Remarks)
            .MaximumLength(1000).WithMessage("Remarks must not exceed 1000 characters.");
    }
}
