using FluentValidation;
using School.Domain.Enums;

namespace School.Application.Features.Attendance.Commands;

public class RecordBulkAttendanceCommandValidator : AbstractValidator<RecordBulkAttendanceCommand>
{
    public RecordBulkAttendanceCommandValidator()
    {
        RuleFor(x => x.ClassRoomId)
            .NotEmpty().WithMessage("Class room ID is required.");

        RuleFor(x => x.Date)
            .NotEmpty().WithMessage("Date is required.")
            .LessThanOrEqualTo(DateOnly.FromDateTime(DateTime.UtcNow))
            .WithMessage("Attendance cannot be recorded for future dates.");

        RuleFor(x => x.Students)
            .NotEmpty().WithMessage("At least one student attendance record is required.");

        RuleForEach(x => x.Students).ChildRules(s =>
        {
            s.RuleFor(x => x.StudentId)
                .NotEmpty().WithMessage("Student ID is required.");

            s.RuleFor(x => x.Status)
                .IsInEnum().WithMessage("Invalid attendance status.");

            s.RuleFor(x => x.Notes)
                .MaximumLength(500).WithMessage("Notes must not exceed 500 characters.");
        });
    }
}
