using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ApplicationBLL.Services;
using ApplicationCommon.DTOs.User;
using ApplicationDAL.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace group_project_thread.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly UserService _userService;
        private readonly AuthService _authService;

        public AuthController(UserService userService, AuthService authService)
        {
            _userService = userService;
            _authService = authService;
        }


        [HttpPost("register")]
        public async Task<AuthUser> Register([FromBody] RegisterUserDTO newUserDto)
        {
            var userModel = await _userService.CreateUser(newUserDto);
            var token = _authService.GenerateAccessToken(userModel.Id, userModel.Username, userModel.Email);

            return new AuthUser()
            {
                User = userModel,
                Token = token
            };
        }
        
        [HttpPost("login")]
        public async Task<AuthUser> Login([FromBody] LoginUserDTO loginUserDto)
        {
            return await _authService.Authorize(loginUserDto);
        }
    }
}
