using KiddooPlaySchool.Domain.Interfaces;
using KiddooPlaySchool.Infrastructure.Data;
using KiddooPlaySchool.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace KiddooPlaySchool.Infrastructure.Extensions;

public static class ServiceExtension
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<AppDbContext>(options =>
            options.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));

        services.AddScoped<IUnitOfWork>(sp => sp.GetRequiredService<AppDbContext>());
        services.AddScoped(typeof(IRepository<>), typeof(BaseRepository<>));

        return services;
    }
}
