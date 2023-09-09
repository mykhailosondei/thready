using ApplicationDAL.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Build.Framework;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace group_project_thread.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PostController : ControllerBase
    {
        // GET: api/<PostController>
        [HttpGet]
        public async Task<IEnumerable<Post>> GetAllPosts()
        {
            return new Post[] { };
        }

        // GET api/<PostController>/5
        [HttpGet("{id}")]
        public async Task<Post> GetPostById(int id)
        {
            return new Post();
        }

        // POST api/<PostController>
        [HttpPost]
        public async Task CreatePost([FromBody] Post post)
        {
        }
        [HttpPost("{id}/likePost")]
        public async Task LikePost(int id)
        {

        }
        [HttpPost("{id}/bookmarkPost")]
        public async Task BookmarkPost(int id)
        {

        }
        // PUT api/<PostController>/5
        [HttpPut("{id}")]
        public async Task PutPost(int id, [FromBody] Post post)
        {
        }

        // DELETE api/<PostController>/5
        [HttpDelete("{id}")]
        public async Task DeletePost(int id)
        {
        }

        [HttpPost("{id}/unlikePost")]
        public async Task UnlikePost(int id)
        {

        }

        [HttpPost("{id}/removeFromBookmarks")]
        public async Task RemoveFromBookmarksPost(int id)
        {

        }
        [HttpPost("{id}/repost")]
        public async Task Repost(int id)
        {

        }
    }
}
