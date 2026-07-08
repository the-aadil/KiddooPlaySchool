using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using School.Application.Interfaces;
using School.Infrastructure.Persistence;
using School.Infrastructure.Persistence.Interceptors;
using School.Infrastructure.Services;

namespace School.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructureLayer(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddScoped<ICurrentUserContext, CurrentUserContext>();
        services.AddScoped<IJwtService, JwtService>();

        services.AddScoped<AuditableEntitySaveChangesInterceptor>();

        services.AddDbContext<IApplicationDbContext, ApplicationDbContext>(options =>
            options.UseSqlServer(
                configuration.GetConnectionString("DefaultConnection"),
                b => b.MigrationsAssembly(typeof(ApplicationDbContext).Assembly.FullName)));

        return services;
    }
}
