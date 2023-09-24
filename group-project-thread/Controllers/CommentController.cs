﻿using ApplicationBLL.Services;
using ApplicationCommon.DTOs.Comment;
using ApplicationCommon.Interfaces;
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
        private readonly IUserIdGetter _userIdGetter;

        public CommentController(CommentService commentService, LikeService likeService, IUserIdGetter userIdGetter)
        {
            _commentService = commentService;
            _likeService = likeService;
            _userIdGetter = userIdGetter;
        }

        // GET api/<CommentController>/5
        [HttpGet("{id}")]
        [AllowAnonymous]
        public async Task<CommentDTO> GetCommentById(int id)
        {
            return await _commentService.GetCommentById(id);
        }

        // POST api/<CommentController>
        [HttpPost]
        public async Task PostComment([FromBody] CommentCreateDTO comment)
        {
            int authorId = _userIdGetter.CurrentId;
            comment.UserId = authorId;
            await _commentService.PostComment(comment);
        }
        [HttpPost("{id}/likeComment")]
        public async Task LikeComment(int id)
        {
            int authorId = _userIdGetter.CurrentId;
            await _likeService.LikeComment(id, authorId);
        }

        // PUT api/<CommentController>/5
        [HttpPut("{id}")]
        public async Task PutComment(int id, [FromBody] CommentDTO comment)
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
            int authorId = _userIdGetter.CurrentId;
            await _likeService.DislikeComment(id, authorId);
        }
    }
}
