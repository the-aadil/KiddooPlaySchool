using FluentValidation;

namespace School.Application.Features.Auth.Commands;

public class RegisterAdminCommandValidator : AbstractValidator<RegisterAdminCommand>
{
    public RegisterAdminCommandValidator()
    {
        RuleFor(x => x.Username)
            .NotEmpty().WithMessage("Username is required.")
            .MaximumLength(50);

        RuleFor(x => x.Email)
            .NotEmpty().WithMessage("Email is required.")
            .EmailAddress().WithMessage("A valid email is required.")
            .MaximumLength(100);

        RuleFor(x => x.Password)
            .NotEmpty().WithMessage("Password is required.")
            .MinimumLength(6).WithMessage("Password must be at least 6 characters.")
            .MaximumLength(100);

        RuleFor(x => x.FirstName)
            .NotEmpty().WithMessage("First name is required.")
            .MaximumLength(50);

        RuleFor(x => x.LastName)
            .NotEmpty().WithMessage("Last name is required.")
            .MaximumLength(50);

        RuleFor(x => x.MobileNumber)
            .NotEmpty().WithMessage("Mobile number is required.")
            .MaximumLength(20);

        RuleFor(x => x.Department)
            .NotEmpty().WithMessage("Department is required.")
            .MaximumLength(100);

        RuleFor(x => x.Designation)
            .MaximumLength(100);
    }
}
