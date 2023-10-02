using ApplicationBLL.QueryRepositories;
using ApplicationBLL.Services;
using ApplicationCommon.DTOs.User;
using ApplicationCommon.Interfaces;
using ApplicationDAL.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace group_project_thread.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class UserController : ControllerBase
    {

        private readonly UserQueryRepository _userQueryRepository;
        private readonly UserService _userService;
        private readonly IUserIdGetter _userIdGetter;
        private readonly EmailValidatorService _emailValidatorService;
        private readonly UsernameValidatorService _usernameValidatorService;
        
        public UserController(UserService userService, IUserIdGetter userIdGetter, EmailValidatorService emailValidatorService, UsernameValidatorService usernameValidatorService, UserQueryRepository userQueryRepository)
        {
            _userService = userService;
            _userIdGetter = userIdGetter;
            _emailValidatorService = emailValidatorService;
            _usernameValidatorService = usernameValidatorService;
            _userQueryRepository = userQueryRepository;
        }

        // GET: api/<UserController>
        [HttpGet]
        [AllowAnonymous]
        public async Task<IEnumerable<UserDTO>> GetAllUsers()
        {
            return await _userQueryRepository.GetAllUsers();
        }

        // GET api/<UserController>/5
        [HttpGet("{id}")]
        [AllowAnonymous]
        public async Task<UserDTO> GetUserById(int id)
        {
            return await _userQueryRepository.GetUserById(id);
        }
        
        [HttpGet("currentUser")]
        [Authorize]
        public async Task<UserDTO> GetCurrentUser()
        {
            return await _userQueryRepository.GetUserById(_userIdGetter.CurrentId);
        }

        //GET: api/User/isEmailAvailable?email={your email}
        [HttpGet("isEmailAvailable")]
        [AllowAnonymous]
        public async Task<bool> IsEmailAvailable(string email)
        {
            return await _emailValidatorService.IsEmailAvailable(email);
        }
        
        //GET: api/User/isUsernameAvailable?username={your username}
        [HttpGet("isUsernameAvailable")]
        [AllowAnonymous]
        public async Task<bool> IsUsernameAvailable(string username)
        {
            return await _usernameValidatorService.IsUsernameAvailable(username);
        }
        
        [HttpPost("{id}/follow")]
        public async Task Follow(int id)
        {
            int currentUserId = _userIdGetter.CurrentId; 
            await _userService.Follow(id, currentUserId);
        }

        // PUT api/<UserController>/5
        [HttpPut("{id}")]
        public async Task PutUser(int id, [FromBody] UserDTO value)
        {
            await _userService.PutUser(id, value);
        }

        // DELETE api/<UserController>/5
        [HttpDelete("{id}")]
        public async Task DeleteUser(int id)
        {
            if(id != _userIdGetter.CurrentId) return;
            await _userService.DeleteUser(id);
        }
        [HttpPost("{id}/unfollow")]
        public async Task Unfollow(int id)
        {
            int currentUserId = _userIdGetter.CurrentId;
            await _userService.Unfollow(id, currentUserId);
        }
    }
}
