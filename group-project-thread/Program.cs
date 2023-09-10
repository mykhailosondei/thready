using System.Text;
using ApplicationBLL.Extentions;
using ApplicationBLL.ProfilesForAutoMapper;
using ApplicationDAL.Context;
using Microsoft.EntityFrameworkCore;


using AutoMapper;
using group_project_thread.Middlewares;
using Microsoft.IdentityModel.Tokens;

var builder = WebApplication.CreateBuilder(args);

ConfigurationManager config = builder.Configuration;

// Add services to the container.

builder.Services.AddControllersWithViews();

builder.Services.ConfigureCustomServices();
builder.Services.AddAutoMapperProfiles();

builder.Services.AddAuthentication().AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuerSigningKey = true,
        ValidateIssuer = false,
        ValidateAudience = false,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config["JwtKey"]))
    };
});

builder.Services.AddScoped<ConfigurationManager>(c => config);
builder.Services.AddDbContext<ApplicationContext>(options =>
{
    options.UseNpgsql(config.GetConnectionString("Default"));
});
builder.Services.AddCors(options => options.AddPolicy(name: "Frontend", policy =>
    {
        policy.WithOrigins("https://localhost:44498").AllowAnyHeader().AllowAnyMethod();
    }
));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseCors("Frontend");
app.UseAuthentication();
app.UseAuthorization();
app.UseMiddleware<UserIdSaverMiddleware>();


app.MapControllerRoute(
    name: "default",
    pattern: "{controller}/{action=Index}/{id?}");

app.MapFallbackToFile("index.html");

app.Run();