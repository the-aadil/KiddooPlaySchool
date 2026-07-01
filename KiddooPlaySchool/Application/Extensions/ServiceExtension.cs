using System.Reflection;
using FluentValidation;
using FluentValidation.AspNetCore;
using KiddooPlaySchool.Application.Interfaces;
using KiddooPlaySchool.Application.Services;
using Microsoft.Extensions.DependencyInjection;

namespace KiddooPlaySchool.Application.Extensions;

public static class ServiceExtension
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddAutoMapper(Assembly.GetExecutingAssembly());
        services.AddFluentValidationAutoValidation();
        services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());

        services.AddScoped<IStudentService, StudentService>();

        return services;
    }
}
