using System.Net.Http.Headers;
using System.Net.Http.Json;
using ApplicationBLL.Extentions;
using ApplicationCommon.DTOs.User;
using ApplicationDAL.Context;
using ApplicationDAL.Entities;
using group_project_thread.Controllers;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Xunit.Abstractions;
using Xunit.Sdk;

namespace BLLIntegrationTests;

public class IntegrationTest
{
    protected readonly HttpClient TestClient;
    protected readonly AuthController _authController;
    protected readonly ITestOutputHelper _outputHelper = new TestOutputHelper();
    
    public IntegrationTest()
    {
        var appFactory = new WebApplicationFactory<Program>()
            .WithWebHostBuilder(builder =>
            {
                builder.ConfigureServices(services =>
                {
                    services.RemoveAll(typeof(ApplicationContext));
                    services.RemoveAll(typeof(DbContextOptions<ApplicationContext>));
                    services.AddDbContext<ApplicationContext>(options =>
                    {
                        options.UseNpgsql("Host=35.226.61.207; Database=testDb; Username=postgres; Password=mgdI-Fot$4]+Fl:P; IncludeErrorDetail=true");
                        options.EnableSensitiveDataLogging();
                        options.UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);
                    });
                });
            });
        TestClient = appFactory.CreateClient();
    }

    protected async Task AuthenticateAsync()
    {
        TestClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", await GetJwtAsync());
    }

    private async Task<string> GetJwtAsync()
    {
        var response = await TestClient.PostAsJsonAsync("/api/auth/login", new LoginUserDTO()
        {
            Email = "littlebobus2005@gmail.com",
            Password = "testpassword",
            });
        var registrationResponse = await response.Content.ReadAsAsync<AuthUser>();
        return registrationResponse.Token;
    }
}