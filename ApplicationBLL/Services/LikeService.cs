using ApplicationBLL.QueryRepositories;
using ApplicationBLL.Services.Abstract;
using ApplicationDAL.Context;
using ApplicationDAL.Entities;
using AutoMapper;

namespace ApplicationBLL.Services;

public class LikeService : BaseService
{
    private readonly PostQueryRepository _postQueryRepository;
    private readonly UserQueryRepository _userQueryRepository;
    private readonly CommentQueryRepository _commentQueryRepository;
    
    public LikeService(ApplicationContext applicationContext, IMapper mapper, PostQueryRepository postQueryRepository, UserQueryRepository userQueryRepository, CommentQueryRepository commentQueryRepository) : base(applicationContext, mapper)
    {
        _postQueryRepository = postQueryRepository;
        _userQueryRepository = userQueryRepository;
        _commentQueryRepository = commentQueryRepository;
    }
    
    public async Task LikePost(int postId, int authorId)
    {
        var post = await _postQueryRepository.GetPostById(postId);
        var author = await _userQueryRepository.GetUserById(authorId);
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
        var post = await _postQueryRepository.GetPostById(postId);
        var author = await _userQueryRepository.GetUserById(authorId);
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
        var comment = await _commentQueryRepository.GetCommentByIdPlain(commentId);
        var author = await _userQueryRepository.GetUserById(authorId);
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
        var comment = await _commentQueryRepository.GetCommentByIdPlain(commentId);
        var author = await _userQueryRepository.GetUserById(authorId);
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