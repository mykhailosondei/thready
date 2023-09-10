using System.IdentityModel.Tokens.Jwt;
using ApplicationBLL.Services;
using AutoMapper;
using Castle.Core.Configuration;
using Microsoft.IdentityModel.Tokens;
using Moq;

namespace BLLTests;

public class AuthServiceTests
{
    private readonly AuthService _authService;
    private readonly Mock<IMapper> _mapperMock;
    private readonly Mock<IConfiguration> _configurationMock;
        
    [Fact]
    public void GenerateAccessToken_ShouldReturnSameClaims()
    {
        //Arrange
        int id = 12;
        string username = "DeMarcus";
        string email = "bop@mail.com";

        //Act
        var token = _authService.GenerateAccessToken(id, username, email);
        var jsonToken = new JwtSecurityTokenHandler().ReadJwtToken(token);
        //Assert
        Assert.Equal(jsonToken.Id, id.ToString());
        Assert.Equal(jsonToken.Claims.FirstOrDefault(x=>x.Type == "sub")?.Value, username);
        Assert.Equal(jsonToken.Claims.FirstOrDefault(x => x.Type == "email")?.Value, email);
    }
}