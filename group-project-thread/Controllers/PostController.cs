using ApplicationBLL.Services;
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

        private readonly PostService _postService;
        private readonly LikeService _likeService;

        public PostController(PostService postService, LikeService likeService)
        {
            _postService = postService;
            _likeService = likeService;
        }

        // GET: api/<PostController>
        [HttpGet]
        public async Task<IEnumerable<Post>> GetAllPosts()
        {
            return await _postService.GetAllPosts();
        }

        // GET api/<PostController>/5
        [HttpGet("{id}")]
        public async Task<Post> GetPostById(int id)
        {
            return await _postService.GetPostById(id);
        }

        // POST api/<PostController>
        [HttpPost]
        public async Task CreatePost([FromBody] Post post)
        {
            await _postService.CreatePost(post);
        }
        
        [HttpPost("{id}/likePost")]
        public async Task LikePost(int id)
        {
            int authorId = 0; //TODO: implement authorId getting from httpContext
            await _likeService.LikePost(id, authorId);
        }
        
        [HttpPost("{id}/bookmarkPost")]
        public async Task BookmarkPost(int id)
        {
            int authorId = 0; //TODO: implement authorId getting from httpContext
            await _postService.BookmarkPost(id, authorId);
        }
        
        // PUT api/<PostController>/5
        [HttpPut("{id}")]
        public async Task PutPost(int id, [FromBody] Post post)
        {
            await _postService.PutPost(id, post);
        }

        // DELETE api/<PostController>/5
        [HttpDelete("{id}")]
        public async Task DeletePost(int id)
        {
            await _postService.DeletePost(id);
        }

        [HttpPost("{id}/unlikePost")]
        public async Task UnlikePost(int id)
        {
            int authorId = 0; //TODO: implement authorId getting from httpContext
            await _likeService.DislikePost(id, authorId);
        }

        [HttpPost("{id}/removeFromBookmarks")]
        public async Task RemoveFromBookmarksPost(int id)
        {
            int authorId = 0; //TODO: implement authorId getting from httpContext
            await _postService.RemoveFromBookmarksPost(id, authorId);
        }
        
        [HttpPost("{id}/repost")]
        public async Task Repost(int id)
        {
            int authorId = 0; //TODO: implement authorId getting from httpContext
            await _postService.Repost(id, authorId);
        }
        
        [HttpPost("{id}/undoRepost")]
        public async Task UndoRepost(int id)
        {
            int authorId = 0; //TODO: implement authorId getting from httpContext
            await _postService.UndoRepost(id, authorId);
        }
    }
}
