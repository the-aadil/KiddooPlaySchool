using System.Net;
using KiddooPlaySchool.Application.DTOs.Common;
using KiddooPlaySchool.Domain.Exceptions;
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

        var (statusCode, message, errors) = context.Exception switch
        {
            EntityNotFoundException enf => ((int)HttpStatusCode.NotFound, enf.Message, (List<string>?)null),
            DomainInvariantViolationException div =>
                ((int)HttpStatusCode.UnprocessableEntity, div.Message, (List<string>?)null),
            UnauthorizedAccessException uae => ((int)HttpStatusCode.Forbidden, uae.Message, (List<string>?)null),
            InvalidOperationException ioe => ((int)HttpStatusCode.Conflict, ioe.Message, (List<string>?)null),
            ArgumentException ae => ((int)HttpStatusCode.BadRequest, ae.Message, (List<string>?)null),
            _ => ((int)HttpStatusCode.InternalServerError, "An internal server error occurred.", (List<string>?)null)
        };

        var response = ApiResponse.Fail(message, errors);

        context.Result = new ObjectResult(response)
        {
            StatusCode = statusCode
        };
    }
}
