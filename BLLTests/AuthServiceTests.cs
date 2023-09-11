using System.IdentityModel.Tokens.Jwt;
using ApplicationBLL.Services;
using ApplicationCommon.DTOs.User;
using ApplicationDAL.Context;
using ApplicationDAL.Entities;
using AutoMapper;
using System.Data.Entity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;
using Microsoft.Extensions.Configuration;
using Moq;

namespace BLLTests;

public class AuthServiceTests
{
    private readonly AuthService _authService;
    private readonly Mock<ApplicationContext> _applicationContextMock = new();
    private readonly Mock<IMapper> _mapperMock = new();
    private readonly Mock<IConfiguration> _configurationMock = new();

    public AuthServiceTests()
    {
        _authService = new AuthService(_applicationContextMock.Object, _configurationMock.Object,
            _mapperMock.Object);
    }
    
    [Fact]
    public void GenerateAccessToken_ShouldYieldValidClaims()
    {
        //Arrange
        int id = 17;
        string userName = "DeMarcus";
        string email = "Mark1234";
        _configurationMock.Setup(c => c["JwtKey"])
            .Returns("Super secret key that will never be stored here in the production environment");
        //Act
        string? token = _authService.GenerateAccessToken(id, userName, email);
        var jwtToken = new JwtSecurityTokenHandler().ReadJwtToken(token);
        //Assert
        Assert.Equal(id.ToString(), jwtToken.Claims.FirstOrDefault(c => c.Type == "id")?.Value);
        Assert.Equal(userName, jwtToken.Claims.FirstOrDefault(c => c.Type == "sub")?.Value);
        Assert.Equal(email, jwtToken.Claims.FirstOrDefault(c => c.Type == "email")?.Value);
    }

    [Fact]
    public void Authorize_ShouldThrowException_OnInvalidEmail()
    {
        //Arrange
        var loginCreds = new LoginUserDTO
        {
            Email = "wrong",
            Password = "Any"
        };
        bool? result = null;
        _applicationContextMock.Setup(c => c.Users).Returns(new InternalDbSet<User>(_applicationContextMock.Object, "User"));
        
        //Act
        try
        {
            var authorizeTry = _authService.Authorize(loginCreds).Result;
        }
        catch (Exception e)
        {
            result = true;
        }
        //Assert
        Assert.Equal(true, result);
    }
}