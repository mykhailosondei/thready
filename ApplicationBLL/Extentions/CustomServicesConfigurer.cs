using System.Reflection;
using ApplicationBLL.Logic;
using ApplicationBLL.ProfilesForAutoMapper;
using ApplicationBLL.QueryRepositories;
using ApplicationBLL.Services;
using ApplicationCommon.DTOs.Comment;
using ApplicationCommon.DTOs.Post;
using ApplicationCommon.DTOs.User;
using ApplicationCommon.Interfaces;
using ApplicationDAL.Context;
using Microsoft.Extensions.DependencyInjection;
using AutoMapper;
using FluentValidation;
using group_project_thread.Validators;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace ApplicationBLL.Extentions;

public static class CustomServicesConfigurer
{
    public static void ConfigureCustomServices(this IServiceCollection services)
    {
        services.AddScoped<LikeService>();
        services.AddTransient<IValidator<PostDTO>, PostDTOValidator>();
        services.AddTransient<IValidator<CommentDTO>, CommentDTOValidator>();
        services.AddTransient<IValidator<UserDTO>, UserDTOValidator>();
        services.AddTransient<IValidator<RegisterUserDTO>, RegisterUserDTOValidator>();
        services.AddTransient<IValidator<LoginUserDTO>, LoginUserDTOValidator>();
        services.AddScoped<UserQueryRepository>();
        services.AddScoped<PostQueryRepository>();
        services.AddScoped<CommentQueryRepository>();
        services.AddScoped<UserService>();
        services.AddScoped<PostService>();
        services.AddScoped<CommentService>();
        services.AddScoped<AuthService>();
        services.AddScoped<EmailValidatorService>();
        services.AddScoped<UsernameValidatorService>();
        services.AddLogging();
        

        services.AddScoped<CurrentUserIdProvider>();
        services.AddScoped<IUserIdSetter>(provider => provider.GetService<CurrentUserIdProvider>());
        services.AddScoped<IUserIdGetter>(provider => provider.GetService<CurrentUserIdProvider>());
    }

    public static void AddAutoMapperProfiles(this IServiceCollection services)
    {
        services.AddAutoMapper(cfg =>
        {
            cfg.AddProfile<UserProfile>();
        }, Assembly.GetExecutingAssembly());
    }
}