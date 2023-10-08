using ApplicationBLL.Services.SearchLogic;
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
        private readonly PostsFromIndexReader _postsFromIndexReader;

        public SearchController(PostsFromIndexReader postsFromIndexReader)
        {
            _postsFromIndexReader = postsFromIndexReader;
        }

        [HttpGet("Users")]
        public IEnumerable<UserDTO> GetUsers(string query)
        {
            return new UserDTO[] { };
        }
        
        //GET: api/search/posts?query={your query}
        [HttpGet("Posts")]
        public async Task <IEnumerable<PostDTO>> GetPosts(string query)
        {
            return await _postsFromIndexReader.GetPosts(query);
        }
    }
}
