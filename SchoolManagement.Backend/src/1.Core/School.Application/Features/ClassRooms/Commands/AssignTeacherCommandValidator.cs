using FluentValidation;

namespace School.Application.Features.ClassRooms.Commands;

public class AssignTeacherCommandValidator : AbstractValidator<AssignTeacherCommand>
{
    public AssignTeacherCommandValidator()
    {
        RuleFor(x => x.TeacherProfileId)
            .NotEmpty().WithMessage("Teacher profile ID is required.");

        RuleFor(x => x.ClassRoomId)
            .NotEmpty().WithMessage("Class room ID is required.");
    }
}
