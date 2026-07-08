using FluentValidation;
using KiddooPlaySchool.Application.DTOs.ClassRoom;

namespace KiddooPlaySchool.Application.Validators;

public class AssignTeacherValidator : AbstractValidator<AssignTeacherRequest>
{
    public AssignTeacherValidator()
    {
        RuleFor(x => x.TeacherProfileId)
            .NotEmpty().WithMessage("Teacher profile ID is required.");

        RuleFor(x => x.ClassRoomId)
            .NotEmpty().WithMessage("Class room ID is required.");
    }
}
