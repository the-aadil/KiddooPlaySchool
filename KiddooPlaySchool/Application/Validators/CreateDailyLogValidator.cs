using FluentValidation;
using KiddooPlaySchool.Application.DTOs.Attendance;
using KiddooPlaySchool.Domain.Enums;

namespace KiddooPlaySchool.Application.Validators;

public class CreateDailyLogValidator : AbstractValidator<CreateDailyLogRequest>
{
    public CreateDailyLogValidator()
    {
        RuleFor(x => x.StudentId)
            .NotEmpty().WithMessage("Student ID is required.");

        RuleFor(x => x.ActivityType)
            .IsInEnum().WithMessage("Invalid activity type.");

        RuleFor(x => x.LogTimestamp)
            .NotEmpty().WithMessage("Log timestamp is required.");

        RuleFor(x => x.Visibility)
            .IsInEnum().WithMessage("Invalid visibility setting.");

        RuleFor(x => x.Payload)
            .MaximumLength(2000).WithMessage("Payload must not exceed 2000 characters.");

        RuleFor(x => x.Remarks)
            .MaximumLength(1000).WithMessage("Remarks must not exceed 1000 characters.");

        RuleFor(x => x.MediaUrls)
            .MaximumLength(2000).WithMessage("Media URLs must not exceed 2000 characters.");
    }
}
