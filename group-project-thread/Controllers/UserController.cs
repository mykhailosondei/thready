using ApplicationBLL.Services;
using ApplicationDAL.Entities;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace group_project_thread.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {

        private readonly UserService _userService;

        public UserController(UserService userService)
        {
            _userService = userService;
        }

        // GET: api/<UserController>
        [HttpGet]
        public async Task<IEnumerable<User>> GetAllUsers()
        {
            return await _userService.GetAllUsers();
        }

        // GET api/<UserController>/5
        [HttpGet("{id}")]
        public async Task<User> GetUserById(int id)
        {
            return await _userService.GetUserById(id);
        }
        
        [HttpPost("{id}/follow")]
        public async Task Follow(int id)
        {
            int currentUserId = 0; //TODO: implement currentUserId getting from httpContext
            await _userService.Follow(id, currentUserId);
        }

        // PUT api/<UserController>/5
        [HttpPut("{id}")]
        public async Task PutUser(int id, [FromBody] User value)
        {
            await _userService.PutUser(id, value);
        }

        // DELETE api/<UserController>/5
        [HttpDelete("{id}")]
        public async Task DeleteUser(int id)
        {
            await _userService.DeleteUser(id);
        }
        [HttpPost("{id}/unfollow")]
        public async Task Unfollow(int id)
        {
            int currentUserId = 0; //TODO: implement currentUserId getting from httpContext
            await _userService.Follow(id, currentUserId);
        }
    }
}
