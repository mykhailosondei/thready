using ApplicationDAL.Entities;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace group_project_thread.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SearchController : ControllerBase
    {
        [HttpGet("Users")]
        public IEnumerable<User> GetUsers(string query)
        {
            return new User[] { };
        }
        [HttpGet("Posts")]
        public IEnumerable<Post> GetPosts(string query)
        {
            return new Post[] { };
        }




    }
}
