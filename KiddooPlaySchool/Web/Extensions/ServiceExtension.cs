using KiddooPlaySchool.Web.Filters;
using KiddooPlaySchool.Web.Middleware;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace KiddooPlaySchool.Web.Extensions;

public static class ServiceExtension
{
    public static IServiceCollection AddWebLayer(this IServiceCollection services)
    {
        services.AddControllers(options =>
        {
            options.Filters.Add<ExceptionFilter>();
        });

        services.AddCors(options =>
        {
            options.AddPolicy("AllowAll", policy =>
            {
                policy.AllowAnyOrigin()
                      .AllowAnyMethod()
                      .AllowAnyHeader();
            });
        });

        return services;
    }

    public static IApplicationBuilder UseWebLayer(this IApplicationBuilder app)
    {
        app.UseCors("AllowAll");
        app.UseMiddleware<RequestLoggingMiddleware>();

        return app;
    }
}
