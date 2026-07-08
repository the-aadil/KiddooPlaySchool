using FluentValidation;
using School.Domain.Enums;

namespace School.Application.Features.ClassRooms.Commands;

public class CreateClassRoomCommandValidator : AbstractValidator<CreateClassRoomCommand>
{
    public CreateClassRoomCommandValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Class room name is required.")
            .MaximumLength(100);

        RuleFor(x => x.Capacity)
            .GreaterThan(0).WithMessage("Capacity must be greater than 0.")
            .LessThanOrEqualTo(200).WithMessage("Capacity must not exceed 200.");

        RuleFor(x => x.AgeGroup)
            .IsInEnum().WithMessage("Invalid age group specified.");
    }
}
