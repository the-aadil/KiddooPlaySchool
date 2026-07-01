using FluentValidation;
using KiddooPlaySchool.Application.DTOs;

namespace KiddooPlaySchool.Application.Validators;

public class StudentValidator : AbstractValidator<StudentDto>
{
    public StudentValidator()
    {
        RuleFor(x => x.Email)
            .NotEmpty()
            .EmailAddress();

        RuleFor(x => x.FullName)
            .NotEmpty()
            .MaximumLength(100);
    }
}
