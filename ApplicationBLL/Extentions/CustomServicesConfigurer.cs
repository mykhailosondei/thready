using System.Reflection;
using ApplicationBLL.Logic;
using ApplicationBLL.ProfilesForAutoMapper;
using ApplicationBLL.Services;
using ApplicationCommon.DTOs.Post;
using ApplicationCommon.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using AutoMapper;
using FluentValidation;
using group_project_thread.Validators;

namespace ApplicationBLL.Extentions;

public static class CustomServicesConfigurer
{
    public static void ConfigureCustomServices(this IServiceCollection services)
    {
        services.AddScoped<LikeService>();
        services.AddTransient<IValidator<PostDTO>, PostDTOValidator>();
        services.AddScoped<UserService>();
        services.AddScoped<PostService>();
        services.AddScoped<CommentService>();
        services.AddScoped<AuthService>();
        services.AddScoped<EmailValidatorService>();
        services.AddScoped<UsernameValidatorService>();
        

        services.AddScoped<CurrentUserIdProvider>();
        services.AddTransient<IUserIdSetter>(provider => provider.GetService<CurrentUserIdProvider>());
        services.AddTransient<IUserIdGetter>(provider => provider.GetService<CurrentUserIdProvider>());
    }

    public static void AddAutoMapperProfiles(this IServiceCollection services)
    {
        services.AddAutoMapper(cfg =>
        {
            cfg.AddProfile<UserProfile>();
        }, Assembly.GetExecutingAssembly());
    }
}