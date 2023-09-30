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

namespace BLLIntegrationTests;

public class IntegrationTest
{
    protected readonly HttpClient TestClient;
    protected readonly AuthController _authController;
    
    public IntegrationTest()
    {
        var appFactory = new WebApplicationFactory<Program>()
            .WithWebHostBuilder(builed =>
            {
                builed.ConfigureServices(services =>
                {
                    services.RemoveAll(typeof(ApplicationContext));
                    services.ConfigureCustomServices();
                    services.AddDbContext<ApplicationContext>(options =>
                    {
                        options.UseInMemoryDatabase("appDb");
                    });
                });
            });
        TestClient = appFactory.CreateClient();
    }

    protected async Task AuthenticateAsync()
    {
        TestClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("bearer", await GetJwtAsync());
    }

    private async Task<string> GetJwtAsync()
    {
        var response = await TestClient.PostAsJsonAsync("https://localhost:7153/api/auth/register", new RegisterUserDTO()
        {
            Username = "bobusbalibobus24",
            Email = "littlebobus2005@gmail.com",
            Password = "testpassword",
            DateOfBirth = DateOnly.FromDateTime(DateTime.UtcNow)
            
        });
        var registrationResponse = await response.Content.ReadAsAsync<AuthUser>();
        return registrationResponse.Token;
    }
}