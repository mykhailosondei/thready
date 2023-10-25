using ApplicationBLL.Services.SearchLogic;
using ApplicationCommon.DTOs.Post;
using ApplicationCommon.DTOs.User;
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
        private readonly IndexedContentReader _indexedContentReader;

        public SearchController(IndexedContentReader indexedContentReader)
        {
            _indexedContentReader = indexedContentReader;
        }
        
        //GET: api/search/users?query={your query}&lowerCount={lowerCount}&upperCount={upperCount}
        [HttpGet("Users")]
        public async Task<IEnumerable<PageUserDTO>> GetUsers(string query, string lowerCount, string upperCount)
        {
            return await _indexedContentReader.GetUsers(query, int.Parse(lowerCount), int.Parse(upperCount));
        }
        
        //GET: api/search/posts?query={your query}&lowerCount={lowerCount}&upperCount={upperCount}
        [HttpGet("Posts")]
        public async Task<IEnumerable<PostDTO>> GetPosts(string query, string lowerCount, string upperCount)
        {
            return await _indexedContentReader.GetPosts(query, int.Parse(lowerCount), int.Parse(upperCount));
        }
    }
}
