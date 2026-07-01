using System.Net;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace KiddooPlaySchool.Web.Filters;

public class ExceptionFilter : IExceptionFilter
{
    private readonly ILogger<ExceptionFilter> _logger;

    public ExceptionFilter(ILogger<ExceptionFilter> logger)
    {
        _logger = logger;
    }

    public void OnException(ExceptionContext context)
    {
        _logger.LogError(context.Exception, "Unhandled exception occurred");

        var (statusCode, message) = context.Exception switch
        {
            KeyNotFoundException => ((int)HttpStatusCode.NotFound, context.Exception.Message),
            UnauthorizedAccessException => ((int)HttpStatusCode.Forbidden, "Access denied"),
            _ => ((int)HttpStatusCode.InternalServerError, "An internal server error occurred")
        };

        var response = new ObjectResult(new
        {
            error = message,
            statusCode
        })
        {
            StatusCode = statusCode
        };

        context.Result = response;
    }
}
