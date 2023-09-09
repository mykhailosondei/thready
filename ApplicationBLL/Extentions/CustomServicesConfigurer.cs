using ApplicationBLL.Services;
using Microsoft.Extensions.DependencyInjection;

namespace ApplicationBLL.Extentions;

public static class CustomServicesConfigurer
{
    public static void ConfigureCustomServices(this IServiceCollection services)
    {
        services.AddTransient<CommentService>();
        services.AddTransient<LikeService>();
        services.AddTransient<PostService>();
        services.AddTransient<UserService>();
    }
}