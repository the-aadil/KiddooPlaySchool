using System.Net;
using KiddooPlaySchool.Application.DTOs.Common;
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
            UnauthorizedAccessException => ((int)HttpStatusCode.Forbidden, context.Exception.Message),
            InvalidOperationException => ((int)HttpStatusCode.Conflict, context.Exception.Message),
            ArgumentException => ((int)HttpStatusCode.BadRequest, context.Exception.Message),
            _ => ((int)HttpStatusCode.InternalServerError, "An internal server error occurred.")
        };

        var response = ApiResponse.Fail(message);

        context.Result = new ObjectResult(response)
        {
            StatusCode = statusCode
        };
    }
}
