using FluentValidation;
using KiddooPlaySchool.Application.DTOs.Student;

namespace KiddooPlaySchool.Application.Validators;

public class UpdateStudentValidator : AbstractValidator<UpdateStudentRequest>
{
    public UpdateStudentValidator()
    {
        RuleFor(x => x.FirstName)
            .NotEmpty().WithMessage("First name is required.")
            .MaximumLength(50);

        RuleFor(x => x.LastName)
            .NotEmpty().WithMessage("Last name is required.")
            .MaximumLength(50);

        RuleFor(x => x.Email)
            .NotEmpty().WithMessage("Email is required.")
            .EmailAddress().WithMessage("Invalid email format.")
            .MaximumLength(100);

        RuleFor(x => x.Phone)
            .NotEmpty().WithMessage("Phone number is required.")
            .MaximumLength(20);

        RuleFor(x => x.Password)
            .MinimumLength(6).When(x => !string.IsNullOrWhiteSpace(x.Password))
            .WithMessage("Password must be at least 6 characters.");

        RuleFor(x => x.DateOfBirth)
            .LessThan(DateTime.UtcNow).WithMessage("Date of birth must be in the past.");

        RuleFor(x => x.ParentName)
            .MaximumLength(100);

        RuleFor(x => x.ParentPhone)
            .MaximumLength(20);

        RuleFor(x => x.ParentEmail)
            .EmailAddress().When(x => !string.IsNullOrWhiteSpace(x.ParentEmail))
            .WithMessage("Invalid parent email format.")
            .MaximumLength(100);
    }
}
