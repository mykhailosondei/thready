using ApplicationCommon.DTOs.Post;
using ApplicationCommon.DTOs.User;
using ApplicationDAL.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace group_project_thread.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [AllowAnonymous]
    public class SearchController : ControllerBase
    {
        [HttpGet("Users")]
        public IEnumerable<UserDTO> GetUsers(string query)
        {
            return new UserDTO[] { };
        }
        [HttpGet("Posts")]
        public IEnumerable<PostDTO> GetPosts(string query)
        {
            return new PostDTO[] { };
        }
    }
}
