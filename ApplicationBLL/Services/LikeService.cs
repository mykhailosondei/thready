using ApplicationBLL.Services.Abstract;
using ApplicationDAL.Context;
using ApplicationDAL.Entities;
using AutoMapper;

namespace ApplicationBLL.Services;

public class LikeService : BaseService
{
    private readonly PostService _postService;
    private readonly CommentService _commentService;
    private readonly UserService _userService;
    
    public LikeService(ApplicationContext applicationContext, IMapper mapper, PostService postService, CommentService commentService, UserService userService) : base(applicationContext, mapper)
    {
        _postService = postService;
        _commentService = commentService;
        _userService = userService;
    }
    
    public async Task LikePost(int postId, int authorId)
    {
        var post = await _postService.GetPostById(postId);
        var author = await _userService.GetUserById(authorId);
        if (post.LikesIds.Contains(author.Id))
        {
            throw new InvalidOperationException("You already liked this post");
        }
        
        var postEntity = _mapper.Map<Post>(post);

        postEntity.LikesIds.Add(author.Id);

        _applicationContext.Attach(postEntity);
        _applicationContext.Entry(postEntity).Property(p => p.LikesIds).IsModified = true;
        
        await _applicationContext.SaveChangesAsync();
    }

    public async Task DislikePost(int postId, int authorId)
    {
        var post = await _postService.GetPostById(postId);
        var author = await _userService.GetUserById(authorId);
        if (!post.LikesIds.Contains(author.Id))
        {
            throw new InvalidOperationException("You already don't like this post");
        }

        var postEntity = _mapper.Map<Post>(post);

        postEntity.LikesIds.Remove(author.Id);

        _applicationContext.Attach(postEntity);
        _applicationContext.Entry(postEntity).Property(p => p.LikesIds).IsModified = true;

        await _applicationContext.SaveChangesAsync();
    }

    public async Task LikeComment(int commentId, int authorId)
    {
        var comment = await _commentService.GetCommentByIdPlain(commentId);
        var author = await _userService.GetUserById(authorId);
        if (comment.LikesIds.Contains(author.Id))
        {
            throw new InvalidOperationException("You already liked this comment");
        }

        var commentEntity = _mapper.Map<Comment>(comment);
        
        commentEntity.LikesIds.Add(author.Id);

        _applicationContext.Attach(commentEntity);
        _applicationContext.Entry(commentEntity).Property(c => c.LikesIds).IsModified = true;

        await _applicationContext.SaveChangesAsync();
    }
    
    public async Task DislikeComment(int commentId, int authorId)
    {
        var comment = await _commentService.GetCommentByIdPlain(commentId);
        var author = await _userService.GetUserById(authorId);
        if (!comment.LikesIds.Contains(author.Id))
        {
            throw new InvalidOperationException("You already don't like this comment");
        }
        
        var commentEntity = _mapper.Map<Comment>(comment);
        
        commentEntity.LikesIds.Remove(author.Id);

        _applicationContext.Attach(commentEntity);
        _applicationContext.Entry(commentEntity).Property(c => c.LikesIds).IsModified = true;

        await _applicationContext.SaveChangesAsync();
    }
}