using ApplicationBLL.Exceptions;
using ApplicationBLL.Extentions;
using ApplicationBLL.Services.Abstract;
using ApplicationCommon.DTOs.Comment;
using ApplicationCommon.DTOs.Post;
using ApplicationCommon.DTOs.User;
using ApplicationDAL.Context;
using ApplicationDAL.Entities;
using AutoMapper;
using FluentValidation;
using FluentValidation.Results;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Query;
using Microsoft.Extensions.Logging;

namespace ApplicationBLL.Services;

public class CommentService : BaseService
{
    private readonly PostService _postService;
    private readonly UserService _userService;
    private readonly IValidator<CommentDTO> _commentValidator;
    private readonly ILogger<CommentService> _logger;
    
    public CommentService(ApplicationContext applicationContext, IMapper mapper, PostService postService, UserService userService, IValidator<CommentDTO> commentValidator, ILogger<CommentService> logger) : base(applicationContext, mapper)
    {
        _commentValidator = commentValidator;
        _logger = logger;
        _postService = postService;
        _userService = userService;
    }

    protected CommentService() : base(null, null)
    {
        
    }

    public virtual async Task<CommentDTO> GetCommentById(int id)
    {
        int depth = 0;
        int currentCheckingId = id;
        var commentForDepthChecking = await _applicationContext.Comments.AsNoTracking().FirstOrDefaultAsync(c => c.Id == currentCheckingId);
        
        if (commentForDepthChecking == null)
        {
            throw new CommentNotFoundException("Comment with specified id not found");
        }

        while (commentForDepthChecking.CommentId != null)
        {
            depth++;
            currentCheckingId = commentForDepthChecking.CommentId.Value;
            commentForDepthChecking = await _applicationContext.Comments.AsNoTracking().FirstOrDefaultAsync(c => c.Id == currentCheckingId);
        }

        Console.WriteLine(depth);
        
        var comment = await _applicationContext.Comments.Include(c => c.Author).CustomInclude(depth).AsNoTracking().FirstOrDefaultAsync(c => c.Id == id);
        
        return _mapper.Map<CommentDTO>(comment);
    }
    
    public async Task<IEnumerable<CommentDTO>> GetCommentsOfPostId(int postId)
    {
        var postEntity = await _postService.GetPostById(postId);

        var commentsIdsOfPost = postEntity!.CommentsIds;

        List<CommentDTO> result = new();

        foreach (var id in commentsIdsOfPost)
        {
            try
            {
                result.Add(await GetCommentById(id));
            }
            catch (CommentNotFoundException e)
            {
                Console.WriteLine("One or more comments could not be loaded");
                Console.WriteLine(e);
            }
        }
        
        return result;
    }

    private async Task UpdateCommentsIdsInParentPost(CommentDTO comment)
    {
        if (comment.Id == 0)
        {
            throw new InvalidOperationException("Id was not provided");
        }

        if (comment.PostId == null)
        {
            throw new InvalidOperationException("PostId was not provided");
        }

        var postEntity = _mapper.Map<Post>(await _postService.GetPostById(comment.PostId.Value));
        
        postEntity.CommentsIds.Add(comment.Id);

        _applicationContext.Entry(postEntity.Author).State = EntityState.Unchanged;
        _applicationContext.Attach(postEntity);
        _applicationContext.Entry(postEntity).Property(c => c.CommentsIds).IsModified = true;
        await _applicationContext.SaveChangesAsync();
    }

    private async Task UpdateCommentsIdsInParentComment(CommentDTO comment)
    {
        if (comment.Id == 0)
        {
            throw new InvalidOperationException("Id was not provided");
        }

        if (comment.CommentId == null)
        {
            throw new InvalidOperationException("CommentId was not provided");
        }

        var parentCommentEntity = _mapper.Map<Comment>(await GetCommentById(comment.CommentId.Value));
        
        parentCommentEntity.CommentsIds.Add(comment.Id);
        
        _applicationContext.Entry(parentCommentEntity.Author).State = EntityState.Unchanged;
        _applicationContext.Attach(parentCommentEntity);
        _applicationContext.Entry(parentCommentEntity).Property(c => c.CommentsIds).IsModified = true;
        await _applicationContext.SaveChangesAsync();
    }

    private async Task HandleCommentCommenting(CommentDTO commentDTO)
    {
        InitComment(ref commentDTO);

        var commentEntity = _mapper.Map<Comment>(commentDTO);
            
        _applicationContext.Attach(commentEntity.Author);
        
        _applicationContext.Entry(commentEntity.ParentComment!).State = EntityState.Unchanged;
        
        _applicationContext.Comments.Add(commentEntity);
        await _applicationContext.SaveChangesAsync();

        _applicationContext.ChangeTracker.Clear();
            
        commentDTO.Id = commentEntity.Id;

        await UpdateCommentsIdsInParentComment(commentDTO);
    }

    private async Task HandlePostCommenting(CommentDTO commentDTO)
    {
        InitComment(ref commentDTO);

        var commentEntity = _mapper.Map<Comment>(commentDTO);

        LogEntries("2");
        _applicationContext.Attach(commentEntity.Author);
        _applicationContext.Entry(commentEntity.Post!).State = EntityState.Unchanged;
        _applicationContext.Comments.Add(commentEntity);
        await _applicationContext.SaveChangesAsync();

        _applicationContext.ChangeTracker.Clear();

        commentDTO.Id = commentEntity.Id;

        await UpdateCommentsIdsInParentPost(commentDTO);
    }

    public async Task PostComment(CommentCreateDTO newComment)
    {
        var authorDTO = await _userService.GetUserById(newComment.UserId);

        bool DoesPostIdExist = newComment.PostId.HasValue;
        bool DoesCommentIdExist = newComment.CommentId.HasValue;
        
        var commentDTO = _mapper.Map<CommentDTO>(newComment);
        commentDTO.Author = authorDTO;
        ValidationResult validationResult = await _commentValidator.ValidateAsync(commentDTO);

        if (!validationResult.IsValid)
        {
            throw new ValidationException(validationResult.Errors[0].ErrorMessage);
        }
        
        if (DoesPostIdExist)
        {
            var commentedPost = await _postService.GetPostById(commentDTO.PostId!.Value);
            commentDTO.Post = commentedPost;
            
            await HandlePostCommenting(commentDTO);
        }
        else if (DoesCommentIdExist)
        {
            var parentCommentDTO = await GetCommentById(commentDTO.CommentId!.Value);
            commentDTO.ParentComment = parentCommentDTO;
            
            await HandleCommentCommenting(commentDTO);
        }
    }

    private static void InitComment(ref CommentDTO commentDto)
    {
        commentDto.CreatedAt = DateTime.UtcNow;
        commentDto.LikesIds = new List<int>();
        commentDto.CommentsIds = new List<int>();
        commentDto.ViewedBy = new List<int>();
    }

    public async Task PutComment(int id, CommentUpdateDTO commentUpdate)
    {
        var commentDTO = await GetCommentById(id);
        
        ValidationResult validationResult = await _commentValidator.ValidateAsync(commentDTO);

        if (!validationResult.IsValid)
        {
            throw new ValidationException(validationResult.Errors[0].ErrorMessage);
        }

        var commentEntity = _mapper.Map<Comment>(commentDTO);

        _applicationContext.Attach(commentEntity);
        
        await _applicationContext.Entry(commentEntity).Collection(c => c.Images).LoadAsync();

        commentEntity.TextContent = commentUpdate.TextContent;
        
        
        _logger.LogInformation("images: {}", commentEntity.Images.Select(i => i.Url));
        _logger.LogInformation("images2: {}", commentUpdate.Images.Select(i => i.Url));
        

        var imagesToAdd = commentUpdate.Images.ExceptBy(commentEntity.Images.Select(i=>i.Url), i => i.Url).ToList();
        _logger.LogInformation("imagesToAdd: {}", imagesToAdd.Select(i => i.Url));
        var imagesToDelete = commentEntity.Images.ExceptBy(commentUpdate.Images.Select(i=>i.Url), i => i.Url).ToList();
        _logger.LogInformation("imagesToDelete: {}", imagesToDelete.Select(i => i.Url));

        commentEntity.Images.AddRange(imagesToAdd);

        foreach (var image in imagesToDelete)
        {
            commentEntity.Images.Remove(image);
            _applicationContext.Entry(image).State = EntityState.Deleted;
        }
        
        _applicationContext.Entry(commentEntity).Property(c => c.TextContent).IsModified = true;
        _applicationContext.Entry(commentEntity).Collection(c => c.Images).IsModified = true;
        
        await _applicationContext.SaveChangesAsync();
    }

    public void LogEntries(string header = "")
    {
        _logger.LogInformation(header);
        
        foreach (var entry in _applicationContext.ChangeTracker.Entries())
        {
            _logger.LogInformation("Entry: {}", entry);
        }
    }

    public async Task DeleteComment(int id)
    {
        var commentDTO = await GetCommentById(id);
        
        bool DoesPostIdExist = commentDTO.PostId.HasValue;
        bool DoesCommentIdExist = commentDTO.CommentId.HasValue;
        
        ValidationResult validationResult = await _commentValidator.ValidateAsync(commentDTO);

        if (!validationResult.IsValid)
        {
            throw new ValidationException(new EmptyCommentException().Message);
        }

        var commentEntity = _mapper.Map<Comment>(commentDTO);
        
        _applicationContext.Comments.Remove(commentEntity);
        await _applicationContext.SaveChangesAsync();
        
        _applicationContext.ChangeTracker.Clear();
        
        if (DoesPostIdExist)
        {
            var postEntity =_mapper.Map<Post>(await _postService.GetPostById(commentDTO.PostId!.Value));

            postEntity.CommentsIds.Remove(id);
            
            _applicationContext.Entry(postEntity.Author).State = EntityState.Unchanged;
            _applicationContext.Attach(postEntity);
            _applicationContext.Entry(postEntity).Property(c => c.CommentsIds).IsModified = true;
            await _applicationContext.SaveChangesAsync();
        }
        else if(DoesCommentIdExist)
        {
            var parentCommentEntity = _mapper.Map<Comment>(await GetCommentById(commentDTO.CommentId.Value));
        
            parentCommentEntity.CommentsIds.Remove(commentDTO.Id);
        
            _applicationContext.Entry(parentCommentEntity.Author).State = EntityState.Unchanged;
            _applicationContext.Attach(parentCommentEntity);
            _applicationContext.Entry(parentCommentEntity).Property(c => c.CommentsIds).IsModified = true;
            await _applicationContext.SaveChangesAsync();
        }
        
        
    }
}