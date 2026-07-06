using FluentValidation;
using KiddooPlaySchool.Application.DTOs.Teacher;

namespace KiddooPlaySchool.Application.Validators;

public class UpdateTeacherValidator : AbstractValidator<UpdateTeacherRequest>
{
    public UpdateTeacherValidator()
    {
        RuleFor(x => x.FirstName)
            .NotEmpty().WithMessage("First name is required.")
            .MaximumLength(50);

        RuleFor(x => x.LastName)
            .NotEmpty().WithMessage("Last name is required.")
            .MaximumLength(50);

        RuleFor(x => x.MobileNumber)
            .NotEmpty().WithMessage("Mobile number is required.")
            .MaximumLength(20);

        RuleFor(x => x.Gender)
            .NotEmpty().WithMessage("Gender is required.")
            .MaximumLength(10);

        RuleFor(x => x.Qualification)
            .NotEmpty().WithMessage("Qualification is required.")
            .MaximumLength(200);

        RuleFor(x => x.Password)
            .MinimumLength(6).When(x => !string.IsNullOrWhiteSpace(x.Password))
            .WithMessage("Password must be at least 6 characters.");
    }
}
