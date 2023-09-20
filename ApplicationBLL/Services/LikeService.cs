using ApplicationBLL.Services.Abstract;
using ApplicationDAL.Context;
using ApplicationDAL.Entities;
using AutoMapper;

namespace ApplicationBLL.Services;

public class LikeService : BaseService
{
    private readonly PostService _postService;
    private readonly CommentService _commentService;
    
    public LikeService(ApplicationContext applicationContext, IMapper mapper, PostService postService, CommentService commentService) : base(applicationContext, mapper)
    {
        _postService = postService;
        _commentService = commentService;
    }
    
    public async Task LikePost(int postId, int authorId)
    {
        var post = await _postService.GetPostById(postId);
        if (post.LikesIds.Contains(authorId))
        {
            throw new InvalidOperationException("You already liked this post");
        }
        post.LikesIds.Add(authorId);

        _applicationContext.Posts.Update(_mapper.Map<Post>(post));

        await _applicationContext.SaveChangesAsync();
    }

    public async Task DislikePost(int postId, int authorId)
    {
        var post = await _postService.GetPostById(postId);
        if (!post.LikesIds.Contains(authorId))
        {
            throw new InvalidOperationException("You already don't like this post");
        }
        post.LikesIds.Remove(authorId);

        _applicationContext.Posts.Update(_mapper.Map<Post>(post));

        await _applicationContext.SaveChangesAsync();
    }

    public async Task LikeComment(int commentId, int authorId)
    {
        var comment = await _commentService.GetCommentById(commentId);
        if (comment.LikesIds.Contains(authorId))
        {
            throw new InvalidOperationException("You already liked this comment");
        }
        comment.LikesIds.Add(authorId);

        _applicationContext.Comments.Update(_mapper.Map<Comment>(comment));

        await _applicationContext.SaveChangesAsync();
    }
    
    public async Task DislikeComment(int commentId, int authorId)
    {
        var comment = await _commentService.GetCommentById(commentId);
        if (!comment.LikesIds.Contains(authorId))
        {
            throw new InvalidOperationException("You already don't like this comment");
        }
        comment.LikesIds.Remove(authorId);

        _applicationContext.Comments.Update(_mapper.Map<Comment>(comment));

        await _applicationContext.SaveChangesAsync();
    }
}