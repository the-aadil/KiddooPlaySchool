using FluentValidation;
using KiddooPlaySchool.Application.DTOs.ClassRoom;

namespace KiddooPlaySchool.Application.Validators;

public class CreateClassRoomValidator : AbstractValidator<CreateClassRoomRequest>
{
    public CreateClassRoomValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Class room name is required.")
            .MaximumLength(100);

        RuleFor(x => x.Capacity)
            .GreaterThan(0).WithMessage("Capacity must be greater than 0.")
            .LessThanOrEqualTo(100).WithMessage("Capacity must not exceed 100.");
    }
}
