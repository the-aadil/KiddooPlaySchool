using FluentValidation;
using KiddooPlaySchool.Application.DTOs.Attendance;
using KiddooPlaySchool.Domain.Enums;

namespace KiddooPlaySchool.Application.Validators;

public class RecordBulkAttendanceValidator : AbstractValidator<RecordBulkAttendanceRequest>
{
    public RecordBulkAttendanceValidator()
    {
        RuleFor(x => x.ClassRoomId)
            .NotEmpty().WithMessage("Class room ID is required.");

        RuleFor(x => x.Date)
            .NotEmpty().WithMessage("Date is required.")
            .LessThanOrEqualTo(DateOnly.FromDateTime(DateTime.UtcNow))
            .WithMessage("Attendance cannot be recorded for future dates.");

        RuleFor(x => x.Students)
            .NotEmpty().WithMessage("At least one student attendance record is required.");

        RuleForEach(x => x.Students).ChildRules(student =>
        {
            student.RuleFor(s => s.StudentId)
                .NotEmpty().WithMessage("Student ID is required.");

            student.RuleFor(s => s.Status)
                .IsInEnum().WithMessage("Invalid attendance status.");

            student.RuleFor(s => s.Notes)
                .MaximumLength(500).WithMessage("Notes must not exceed 500 characters.");
        });
    }
}
