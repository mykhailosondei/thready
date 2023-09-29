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
            _logger.LogError($"Validation Error message: {ex}");
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
    }
}