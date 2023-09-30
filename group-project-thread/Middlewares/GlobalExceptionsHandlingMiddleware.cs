using System.Net;
using System.Text.Json;
using ApplicationBLL.Exceptions;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using NuGet.Protocol.Plugins;

namespace group_project_thread.Middlewares;

public class GlobalExceptionsHandlingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<GlobalExceptionsHandlingMiddleware> _logger;

    public GlobalExceptionsHandlingMiddleware(ILogger<GlobalExceptionsHandlingMiddleware> logger, RequestDelegate next)
    {
        _logger = logger;
        _next = next;
    } 
        

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (ValidationException validationFailure)
        {
            _logger.LogError($"Validation Error message: {validationFailure}");
            context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
            context.Response.ContentType = "application/json";

            ProblemDetails errorResponse = new()
            {
                Status = (int)HttpStatusCode.BadRequest,
                Type = "Validation Error",
                Title = validationFailure.Message,
                Detail = $"{validationFailure.Message}"
            };

            string json = JsonSerializer.Serialize(errorResponse);

            await context.Response.WriteAsync(json);
        }
        catch (NotFoundException ex)
        {
            _logger.LogError($"Not found error message: {ex}");
            context.Response.StatusCode = (int)HttpStatusCode.NotFound;
            context.Response.ContentType = "application/json";

            ProblemDetails errorResponse = new()
            {
                Status = (int)HttpStatusCode.NotFound,
                Type = "Page not found",
                Title = $"{ex.EntityName} not found",
                Detail = ex.Message
            };

            string json = JsonSerializer.Serialize(errorResponse);

            await context.Response.WriteAsync(json);
        }
        catch (NotAcceptableExceptions ex)
        {
            _logger.LogError($"Not acceptable error message: {ex}");
            context.Response.StatusCode = (int)HttpStatusCode.NotAcceptable;
            context.Response.ContentType = "application/json";

            ProblemDetails errorResponse = new()
            {
                Status = (int)HttpStatusCode.NotAcceptable,
                Type = "An invalid format is specified",
                Title = $"{ex.EntityName} not acceptable",
                Detail = ex.Message
            };

            string json = JsonSerializer.Serialize(errorResponse);

            await context.Response.WriteAsync(json);
        }
        catch (UserAlreadyExistsException ex)
        {
            _logger.LogError($"Already Exists error message: {ex}");
            context.Response.StatusCode = (int)HttpStatusCode.Conflict;
            context.Response.ContentType = "application/json";

            ProblemDetails errorResponse = new()
            {
                Status = (int)HttpStatusCode.Conflict,
                Type = "Already exist",
                Title = "Conflict with the current state of the target resource",
                Detail = ex.Message
            };

            string json = JsonSerializer.Serialize(errorResponse);

            await context.Response.WriteAsync(json);
        }
        catch (InvalidPasswordException ex)
        {
            _logger.LogError($"Unauthorized error message: {ex}");
            context.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
            context.Response.ContentType = "application/json";

            ProblemDetails errorResponse = new()
            {
                Status = (int)HttpStatusCode.Unauthorized,
                Type = "Unauthorized",
                Title = "There was a problem authenticating your request.",
                Detail = ex.Message
            };

            string json = JsonSerializer.Serialize(errorResponse);

            await context.Response.WriteAsync(json);
        }
        catch (Exception ex)
        {
            _logger.LogError($"Internal server error message: {ex}");
            context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
            context.Response.ContentType = "application/json";

            ProblemDetails errorResponse = new()
            {
                Status = (int)HttpStatusCode.InternalServerError,
                Type = "Internal server error",
                Title = "Internal server error",
                Detail = ex.Message
            };

            string json = JsonSerializer.Serialize(errorResponse);

            await context.Response.WriteAsync(json);
        }
    }
}