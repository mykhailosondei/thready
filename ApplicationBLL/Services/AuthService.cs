using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Principal;
using System.Text;
using ApplicationBLL.Services.Abstract;
using ApplicationDAL.Context;
using AutoMapper;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace ApplicationBLL.Services;

public class AuthService : BaseService
{
    private readonly ConfigurationManager _configuration;
    
    public AuthService(ApplicationContext applicationContext, ConfigurationManager configuration, IMapper mapper) : base(applicationContext, mapper)
    {
        _configuration = configuration;
    }


    public string GenerateAccessToken(int id, string userName, string email)
    {
        var identity = new ClaimsIdentity(new GenericIdentity(userName, "Token"), new[]
        {
            new Claim("id", id.ToString())
        });

        var claims = new[]
        {
            new Claim(JwtRegisteredClaimNames.Sub, userName),
            new Claim(JwtRegisteredClaimNames.Email, email),
            identity.FindFirst("id")
        };

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JwtKey"]!));

        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512);

        var jwtSecurityToken = new JwtSecurityToken(
            claims: claims,
            expires: DateTime.Now.AddDays(1),
            signingCredentials: creds
        );

        string jwtValue =  new JwtSecurityTokenHandler().WriteToken(jwtSecurityToken);

        return jwtValue;
    }
}