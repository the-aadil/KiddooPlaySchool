using System.Reflection;
using FluentValidation;
using FluentValidation.AspNetCore;
using KiddooPlaySchool.Application.Interfaces;
using KiddooPlaySchool.Application.Services;
using KiddooPlaySchool.Domain.Interfaces;
using KiddooPlaySchool.Infrastructure.Data;
using KiddooPlaySchool.Infrastructure.Repositories;
using KiddooPlaySchool.Web.Filters;
using KiddooPlaySchool.Web.Middleware;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace KiddooPlaySchool.Shared;

public static class DependencyInjection
{
    public static IServiceCollection AddAllServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<AppDbContext>(options =>
            options.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));

        services.AddScoped<IUnitOfWork>(sp => sp.GetRequiredService<AppDbContext>());
        services.AddScoped(typeof(IRepository<>), typeof(BaseRepository<>));
        services.AddScoped<IStudentService, StudentService>();

        services.AddAutoMapper(Assembly.GetExecutingAssembly());
        services.AddFluentValidationAutoValidation();
        services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());

        services.AddControllers(options =>
        {
            options.Filters.Add<ExceptionFilter>();
        });

        return services;
    }
}
