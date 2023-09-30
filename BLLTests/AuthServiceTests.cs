using System.Data.Entity.Infrastructure;
using System.IdentityModel.Tokens.Jwt;
using System.Linq.Expressions;
using ApplicationBLL.Exceptions;
using ApplicationBLL.Services;
using ApplicationCommon.DTOs.User;
using ApplicationDAL.Context;
using ApplicationDAL.Entities;
using AutoMapper;
using FluentValidation;
using FluentValidation.Results;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;
using Microsoft.EntityFrameworkCore.Query.Internal;
using Microsoft.Extensions.Configuration;
using Moq;
using Moq.EntityFrameworkCore;
using Xunit.Abstractions;
using Xunit.Sdk;

namespace BLLTests;

public class AuthServiceTests
{
    private readonly AuthService _authService;
    private readonly Mock<ApplicationContext> _applicationContextMock = new();
    private readonly Mock<IMapper> _mapperMock = new();
    private readonly Mock<IConfiguration> _configurationMock = new();
    private readonly Mock<IValidator<LoginUserDTO>> _loginUserDTOValidatorMock = new();
    private readonly ITestOutputHelper _outputHelper;

    public AuthServiceTests(ITestOutputHelper output)
    {
        _authService = new AuthService(_applicationContextMock.Object, _configurationMock.Object,
            _mapperMock.Object, _loginUserDTOValidatorMock.Object);
        _outputHelper = output;
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
        
        _applicationContextMock.SetupGet(c => c.Users).ReturnsDbSet(new List<User>());
        _loginUserDTOValidatorMock.Setup(v => v.ValidateAsync(It.IsAny<LoginUserDTO>(), CancellationToken.None))
            .ReturnsAsync(new ValidationResult()
            {
                Errors = new List<ValidationFailure>() {}
            });
        //Act
        
        //Assert
        var ex = Assert.ThrowsAsync<UserNotFoundException>( async () => await _authService.Authorize(loginCreds)).Result;
        _outputHelper.WriteLine("" + ex);
    }
    
    [Fact]
    public void Authorize_ShouldThrowException_OnInvalidEmail1()
    {
        //Arrange
        var loginCreds = new LoginUserDTO
        {
            Email = "wrong",
            Password = "right"
        };
        
        _applicationContextMock.SetupGet(c => c.Users).ReturnsDbSet(new List<User>());
        _loginUserDTOValidatorMock.Setup(v => v.ValidateAsync(It.IsAny<LoginUserDTO>(), CancellationToken.None))
            .ReturnsAsync(new ValidationResult()
            {
                Errors = new List<ValidationFailure>() {new ValidationFailure() {ErrorMessage = "invalid email"}}
            });
        //Act

        //Assert
        var ex = Assert.ThrowsAsync<ValidationException>( async () => await _authService.Authorize(loginCreds)).Result;
        _outputHelper.WriteLine(""+ex);
    }
    
    [Fact]
    public void Authorize_ShouldThrowException_OnInvalidPassword()
    {
        //Arrange
        var loginCreds = new LoginUserDTO
        {
            Email = "right",
            Password = "right"
        };
        var data = new List<User>
        {
            new() { Username = "Mike",Email = "right", PasswordHash = BCrypt.Net.BCrypt.HashPassword("wrong"), Avatar = new Image(){Id = 1,Url = "url"}}
        };
        
        _applicationContextMock.SetupGet(c => c.Users).ReturnsDbSet(data);
        _loginUserDTOValidatorMock.Setup(v => v.ValidateAsync(It.IsAny<LoginUserDTO>(), CancellationToken.None))
            .ReturnsAsync(new ValidationResult()
            {
                Errors = new List<ValidationFailure>() {}
            });
        //Act

        //Assert
        var ex = Assert.ThrowsAsync<InvalidPasswordException>( async () => await _authService.Authorize(loginCreds)).Result;
        _outputHelper.WriteLine(""+ex);
    }
    
    [Fact]
    public void Authorize_ShouldThrowException_OnInvalidPassword1()
    {
        //Arrange
        var loginCreds = new LoginUserDTO
        {
            Email = "right",
            Password = "wrong"
        };
        var data = new List<User>
        {
            new() { Username = "Mike",Email = "right", PasswordHash = BCrypt.Net.BCrypt.HashPassword("wrong"), Avatar = new Image(){Id = 1,Url = "url"}}
        };
        
        _applicationContextMock.SetupGet(c => c.Users).ReturnsDbSet(data);
        _loginUserDTOValidatorMock.Setup(v => v.ValidateAsync(It.IsAny<LoginUserDTO>(), CancellationToken.None))
            .ReturnsAsync(new ValidationResult()
            {
                Errors = new List<ValidationFailure>() {new ValidationFailure() {ErrorMessage = "invalid password"}}
            });
        //Act

        //Assert
        var ex = Assert.ThrowsAsync<ValidationException>( async () => await _authService.Authorize(loginCreds)).Result;
        _outputHelper.WriteLine(""+ex);
    }

    [Fact]
    public void Authorize_ShouldReturnValidAuthUser_OnValidCredentials()
    {
        //Arrange
        var loginCreds = new LoginUserDTO
        {
            Email = "right",
            Password = "right"
        };
        var data = new List<User>
        {
            new() { 
                Username = "Mike",
                Email = "right",
                PasswordHash = BCrypt.Net.BCrypt.HashPassword("right")
            }
        };
        
        _applicationContextMock.SetupGet(c => c.Users).ReturnsDbSet(data);
        _loginUserDTOValidatorMock.Setup(v => v.ValidateAsync(It.IsAny<LoginUserDTO>(), CancellationToken.None))
            .ReturnsAsync(new ValidationResult()
            {
                Errors = new List<ValidationFailure>() {}
            });
        _configurationMock.Setup(c => c["JwtKey"]).Returns("Super secret key that will never be stored here in the production environment");
        _mapperMock.Setup(m => m.Map<UserDTO>(It.IsAny<User>())).Returns((User entity) =>
            new UserDTO() { Email = entity.Email});
        //Act
        
        var authUser = _authService.Authorize(loginCreds).Result;
        
        //Assert
        Assert.Equal("right",authUser.User.Email);
    }
}