using ApplicationDAL.Entities;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace group_project_thread.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CommentController : ControllerBase
    {

        // GET api/<CommentController>/5
        [HttpGet("{id}")]
        public async Task<Comment> GetCommentById(int id)
        {
            return new Comment();
        }

        // POST api/<CommentController>
        [HttpPost]
        public async Task PostComment([FromBody] Comment value)
        {
        }
        [HttpPost("{id}/likeComment")]
        public async Task LikeComment(int id)
        {

        }

        // PUT api/<CommentController>/5
        [HttpPut("{id}")]
        public async Task PutCommentById(int id, [FromBody] Comment value)
        {
        }

        // DELETE api/<CommentController>/5
        [HttpDelete("{id}")]
        public async Task DeleteCommentById(int id)
        {
        }
        [HttpDelete("{id}/unlikeComment")]
        public async Task unlikeComment(int id)
        {

        }
    }
}
