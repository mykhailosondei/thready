using ApplicationCommon.Interfaces;

namespace group_project_thread.Middlewares;

public class UserIdSaverMiddleware
{
    private readonly RequestDelegate _next;

    public UserIdSaverMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext httpContext, IUserIdSetter userIdSetter)
    {
        string? userIdentityClaim = httpContext.User.Claims.FirstOrDefault(c => c.Type == "id")?.Value;
        
        Console.WriteLine(userIdentityClaim);

        if (userIdentityClaim != null && int.TryParse(userIdentityClaim, out int id))
        {
            userIdSetter.CurrentId = id;
        }

        await _next.Invoke(httpContext);
    }
}