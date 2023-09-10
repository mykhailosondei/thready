using System.Reflection;
using ApplicationBLL.ProfilesForAutoMapper;
using ApplicationBLL.Services;
using Microsoft.Extensions.DependencyInjection;
using AutoMapper;

namespace ApplicationBLL.Extentions;

public static class CustomServicesConfigurer
{
    public static void ConfigureCustomServices(this IServiceCollection services)
    {
        services.AddTransient<CommentService>();
        services.AddTransient<LikeService>();
        services.AddTransient<PostService>();
        services.AddTransient<UserService>();
        services.AddTransient<AuthService>();
    }

    public static void AddAutoMapperProfiles(this IServiceCollection services)
    {
        services.AddAutoMapper(cfg =>
        {
            cfg.AddProfile<UserProfile>();
        }, Assembly.GetExecutingAssembly());
    }
}