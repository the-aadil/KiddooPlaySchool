using System.Text.RegularExpressions;
using FluentValidation;
using KiddooPlaySchool.Application.DTOs;

namespace KiddooPlaySchool.Application.Validators;

public partial class RegisterTeacherValidator : AbstractValidator<RegisterTeacherRequest>
{
    public RegisterTeacherValidator()
    {
        RuleFor(x => x.FirstName)
            .NotEmpty().WithMessage("First name is required")
            .MaximumLength(50);

        RuleFor(x => x.LastName)
            .NotEmpty().WithMessage("Last name is required")
            .MaximumLength(50);

        RuleFor(x => x.Username)
            .NotEmpty().WithMessage("Username is required")
            .MinimumLength(4).WithMessage("Username must be at least 4 characters")
            .MaximumLength(30)
            .Matches("^[a-zA-Z0-9_]+$").WithMessage("Username can only contain letters, numbers and underscore");

        RuleFor(x => x.Email)
            .NotEmpty().WithMessage("Email is required")
            .EmailAddress().WithMessage("Invalid email format")
            .MaximumLength(100);

        RuleFor(x => x.Password)
            .NotEmpty().WithMessage("Password is required")
            .MinimumLength(8).WithMessage("Password must be at least 8 characters")
            .MaximumLength(50)
            .Must(HasUpperCase).WithMessage("Password must contain at least one uppercase letter")
            .Must(HasLowerCase).WithMessage("Password must contain at least one lowercase letter")
            .Must(HasDigit).WithMessage("Password must contain at least one digit")
            .Must(HasSpecialChar).WithMessage("Password must contain at least one special character");

        RuleFor(x => x.ConfirmPassword)
            .NotEmpty().WithMessage("Confirm password is required")
            .Equal(x => x.Password).WithMessage("Passwords do not match");

        RuleFor(x => x.MobileNumber)
            .NotEmpty().WithMessage("Mobile number is required")
            .Must(BeValidMobile).WithMessage("Invalid mobile number format");

        RuleFor(x => x.AlternateMobile)
            .Must(BeValidMobile).WithMessage("Invalid alternate mobile number format")
            .When(x => !string.IsNullOrEmpty(x.AlternateMobile));

        RuleFor(x => x.DateOfBirth)
            .NotEmpty().WithMessage("Date of birth is required")
            .Must(BeAtLeast18).WithMessage("Teacher must be at least 18 years old");

        RuleFor(x => x.Gender)
            .NotEmpty().WithMessage("Gender is required")
            .Must(g => g is "Male" or "Female" or "Other").WithMessage("Gender must be Male, Female or Other");

        RuleFor(x => x.Qualification)
            .NotEmpty().WithMessage("Qualification is required")
            .MaximumLength(100);

        RuleFor(x => x.Specialization)
            .MaximumLength(100);

        RuleFor(x => x.Address)
            .MaximumLength(200);

        RuleFor(x => x.City)
            .MaximumLength(50);

        RuleFor(x => x.State)
            .MaximumLength(50);
    }

    private static bool HasUpperCase(string password) =>
        password.Any(char.IsUpper);

    private static bool HasLowerCase(string password) =>
        password.Any(char.IsLower);

    private static bool HasDigit(string password) =>
        password.Any(char.IsDigit);

    private static bool HasSpecialChar(string password) =>
        password.Any(c => !char.IsLetterOrDigit(c));

    private static bool BeAtLeast18(DateTime dateOfBirth)
    {
        var today = DateTime.Today;
        var age = today.Year - dateOfBirth.Year;
        if (dateOfBirth.Date > today.AddYears(-age)) age--;
        return age >= 18;
    }

    private static bool BeValidMobile(string? mobileNumber)
    {
        if (string.IsNullOrEmpty(mobileNumber)) return false;
        return MobileRegex().IsMatch(mobileNumber);
    }

    [GeneratedRegex(@"^\+?\d{10,15}$")]
    private static partial Regex MobileRegex();
}
