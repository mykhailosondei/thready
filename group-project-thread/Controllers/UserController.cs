using ApplicationDAL.Entities;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace group_project_thread.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        // GET: api/<UserController>
        [HttpGet]
        public async Task<IEnumerable<User>> GetAllUsers()
        {
            return new User[] { };
        }

        // GET api/<UserController>/5
        [HttpGet("{id}")]
        public async Task<User> GetUserById(int id)
        {
            return new User();
        }

        // POST api/<UserController>
        [HttpPost]
        public async Task PostUser([FromBody] User value)
        {
        }
        [HttpPost("{id}")]
        public async Task Follow(int id)
        {

        }

        // PUT api/<UserController>/5
        [HttpPut("{id}")]
        public async Task PutUser(int id, [FromBody] User value)
        {
        }

        // DELETE api/<UserController>/5
        [HttpDelete("{id}")]
        public async Task DeleteUser(int id)
        {
        }
        [HttpPost("{id}")]
        public async Task Unfollow(int id)
        {
        }
    }
}
