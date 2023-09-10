using ApplicationBLL.Services;
using ApplicationDAL.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace group_project_thread.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class CommentController : ControllerBase
    {

        private readonly CommentService _commentService;
        private readonly LikeService _likeService;

        public CommentController(CommentService commentService, LikeService likeService)
        {
            _commentService = commentService;
            _likeService = likeService;
        }

        // GET api/<CommentController>/5
        [HttpGet("{id}")]
        [AllowAnonymous]
        public async Task<Comment> GetCommentById(int id)
        {
            return await _commentService.GetCommentById(id);
        }

        // POST api/<CommentController>
        [HttpPost]
        public async Task PostComment([FromBody] Comment comment)
        {
            await _commentService.PostComment(comment);
        }
        [HttpPost("{id}/likeComment")]
        public async Task LikeComment(int id)
        {
            int authorId = 0; //TODO: implement authorId getting from httpContext
            await _likeService.LikeComment(id, authorId);
        }

        // PUT api/<CommentController>/5
        [HttpPut("{id}")]
        public async Task PutComment(int id, [FromBody] Comment comment)
        {
            await _commentService.PutComment(id, comment);
        }

        // DELETE api/<CommentController>/5
        [HttpDelete("{id}")]
        public async Task DeleteComment(int id)
        {
            await _commentService.DeleteComment(id);
        }
        [HttpDelete("{id}/unlikeComment")]
        public async Task UnlikeComment(int id)
        {
            int authorId = 0; //TODO: implement authorId getting from httpContext
            await _likeService.DislikeComment(id, authorId);
        }
    }
}
