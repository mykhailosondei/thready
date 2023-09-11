using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Principal;
using System.Text;
using ApplicationBLL.Services.Abstract;
using ApplicationCommon.DTOs.User;
using ApplicationDAL.Context;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.Extensions.Configuration;

namespace ApplicationBLL.Services;

public class AuthService : BaseService
{
    private readonly IConfiguration _configuration;

    public AuthService(ApplicationContext applicationContext, IConfiguration configuration, IMapper mapper) : base(applicationContext, mapper)
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

        string jwtValue = new JwtSecurityTokenHandler().WriteToken(jwtSecurityToken);

        return jwtValue;
    }

    public async Task<AuthUser> Authorize(LoginUserDTO loginUserDto)
    {
        var userEntity = await _applicationContext.Users
            .Include(u => u.Avatar)
            .FirstOrDefaultAsync(u => u.Email == loginUserDto.Email);

        if (userEntity == null)
        {
            throw new Exception("User not found");
        }

        if (!BCrypt.Net.BCrypt.Verify(loginUserDto.Password, userEntity.PasswordHash))
        {
            throw new Exception("Invalid password");
        }

        var token = GenerateAccessToken(userEntity.Id, userEntity.Username, userEntity.Email);

        var user = _mapper.Map<UserDTO>(userEntity);

        return new AuthUser()
        {
            User = user,
            Token = token
        };
    }
}