using System.Linq.Expressions;
using ApplicationBLL.Exceptions;
using ApplicationBLL.Extentions;
using ApplicationBLL.QueryRepositories;
using ApplicationBLL.Services.Abstract;
using ApplicationCommon.DTOs.Comment;
using ApplicationCommon.DTOs.Image;
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
    private readonly PostQueryRepository _postQueryRepository;
    private readonly UserQueryRepository _userQueryRepository;
    private readonly CommentQueryRepository _commentQueryRepository;
    private readonly IValidator<CommentDTO> _commentValidator;
    private readonly ILogger<CommentService> _logger;
    private readonly IValidator<CommentUpdateDTO> _commentUpdateValidator;
    
    public CommentService(ApplicationContext applicationContext, IMapper mapper, IValidator<CommentDTO> commentValidator,
        ILogger<CommentService> logger, PostQueryRepository postQueryRepository, UserQueryRepository userQueryRepository, 
        CommentQueryRepository commentQueryRepository, IValidator<CommentUpdateDTO> commentUpdateValidator) : base(applicationContext, mapper)
    {
        _commentValidator = commentValidator;
        _logger = logger;
        _postQueryRepository = postQueryRepository;
        _userQueryRepository = userQueryRepository;
        _commentQueryRepository = commentQueryRepository;
        _commentUpdateValidator = commentUpdateValidator;
    }

    protected CommentService(PostQueryRepository postQueryRepository, UserQueryRepository userQueryRepository, CommentQueryRepository commentQueryRepository) : base(null, null)
    {
        _postQueryRepository = postQueryRepository;
        _userQueryRepository = userQueryRepository;
        _commentQueryRepository = commentQueryRepository;
    }
    
    public async Task<IEnumerable<CommentDTO>> GetCommentsOfPostId(int postId)
    {
        var postEntity = await _postQueryRepository.GetPostById(postId);

        var commentsIdsOfPost = postEntity!.CommentsIds;
        
        var comments = await _applicationContext.Comments.Include(c => c.Images).Include(c => c.Author).ThenInclude(u => u.Avatar).Where(c => commentsIdsOfPost.Contains(c.Id)).ToListAsync();
        
        return _mapper.Map<IEnumerable<CommentDTO>>(comments);
    }
    
    public async Task BookmarkComment(int commentId, int userId)
    {
        var userModel = await _userQueryRepository.GetUserById(userId);
        var comment = await _commentQueryRepository.GetCommentByIdPlain(commentId);
        if (comment == null)
        {
            throw new CommentNotFoundException();
        }

        bool isBookmarked = userModel.BookmarkedCommentsIds.Contains(commentId);

        if (isBookmarked)
            throw new InvalidOperationException("Already bookmarked");
        
        var userEntity = _mapper.Map<User>(userModel);
        var commentEntity = _mapper.Map<Comment>(comment);

        commentEntity.Bookmarks++;
        userEntity.BookmarkedCommentsIds.Add(commentId);
        await BookmarkEntitiesSaveChanges(userEntity, commentEntity);
    }
    
    public async Task RemoveFromBookmarksComment(int commentId, int userId)
    {
        var userModel = await _userQueryRepository.GetUserById(userId);
        var comment = await _commentQueryRepository.GetCommentByIdPlain(commentId);
        if (comment == null)
        {
            throw new CommentNotFoundException();
        }

        bool isBookmarked = userModel.BookmarkedCommentsIds.Contains(commentId);

        if (!isBookmarked)
        {
            throw new InvalidOperationException("Not bookmarked");
        }

        var userEntity = _mapper.Map<User>(userModel);
        var commentEntity = _mapper.Map<Comment>(comment);

        commentEntity.Bookmarks--;
        userEntity.BookmarkedCommentsIds.Remove(commentId);
        await BookmarkEntitiesSaveChanges(userEntity, commentEntity);
    }
    
    private async Task BookmarkEntitiesSaveChanges(User userEntity, Comment commentEntity)
    {
        _applicationContext.Attach(userEntity);
        _applicationContext.Entry(userEntity).Property(u => u.BookmarkedCommentsIds).IsModified = true;
        await _applicationContext.SaveChangesAsync();
        _applicationContext.ChangeTracker.Clear();
        _applicationContext.Attach(commentEntity);
        _applicationContext.Entry(commentEntity).Property(p => p.Bookmarks).IsModified = true;
        await _applicationContext.SaveChangesAsync();
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

        var postEntity = _mapper.Map<Post>(await _postQueryRepository.GetPostById(comment.PostId.Value));
        
        postEntity.CommentsIds.Add(comment.Id);
        
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

        var parentCommentEntity = _mapper.Map<Comment>(await _commentQueryRepository.GetCommentByIdPlain(comment.CommentId.Value));
        
        parentCommentEntity.CommentsIds.Add(comment.Id);
        
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
        var authorDTO = await _userQueryRepository.GetUserById(newComment.UserId);

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
            var commentedPost = await _postQueryRepository.GetPostById(commentDTO.PostId!.Value);
            commentDTO.Post = commentedPost;
            
            await HandlePostCommenting(commentDTO);
        }
        else if (DoesCommentIdExist)
        {
            var parentCommentDTO = await _commentQueryRepository.GetCommentByIdPlain(commentDTO.CommentId!.Value);
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
        commentDto.Bookmarks = 0;
    }
    
    private Func<Image, string> imageSelector = image => image.Url;

    public async Task PutComment(int id, CommentUpdateDTO commentUpdate)
    {
        var commentDTO = await _commentQueryRepository.GetCommentByIdPlain(id);
        
        ValidationResult validationResult = await _commentUpdateValidator.ValidateAsync(commentUpdate);

        if (!validationResult.IsValid)
        {
            throw new ValidationException(validationResult.Errors[0].ErrorMessage);
        }

        var commentEntity = _mapper.Map<Comment>(commentDTO);
        var updatedImages = commentUpdate.Images.Select(i => _mapper.Map<Image>(i)).ToList();

        _applicationContext.Attach(commentEntity);
        
        await _applicationContext.Entry(commentEntity).Collection(c => c.Images).LoadAsync();


        var imagesToAdd = updatedImages.ExceptBy(commentEntity.Images.Select(imageSelector), imageSelector).ToList();
        var imagesToDelete = commentEntity.Images.ExceptBy(updatedImages.Select(imageSelector), imageSelector).ToList();

        commentEntity.Images.AddRange(imagesToAdd.DistinctBy(imageSelector));

        foreach (var image in imagesToDelete)
        {
            commentEntity.Images.RemoveAll(i => i.Url == image.Url);
            _applicationContext.Entry(image).State = EntityState.Deleted;
        }
        
        _applicationContext.Entry(commentEntity).Collection(c => c.Images).IsModified = true;
        
        commentEntity.TextContent = commentUpdate.TextContent;
        _applicationContext.Entry(commentEntity).Property(c => c.TextContent).IsModified = true;
        
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
        var commentDTO = await _commentQueryRepository.GetCommentByIdPlain(id);
        
        bool DoesPostIdExist = commentDTO.PostId.HasValue;
        bool DoesCommentIdExist = commentDTO.CommentId.HasValue;
        
        ValidationResult validationResult = await _commentValidator.ValidateAsync(commentDTO);

        if (!validationResult.IsValid)
        {
            throw new EmptyCommentException();
        }

        var commentEntity = _mapper.Map<Comment>(commentDTO);
        
        _applicationContext.Comments.Remove(commentEntity);
        await _applicationContext.SaveChangesAsync();
        
        _applicationContext.ChangeTracker.Clear();
        
        if (DoesPostIdExist)
        {
            var postEntity =_mapper.Map<Post>(await _postQueryRepository.GetPostById(commentDTO.PostId!.Value));

            postEntity.CommentsIds.Remove(id);
            
            _applicationContext.Attach(postEntity);
            _applicationContext.Entry(postEntity).Property(c => c.CommentsIds).IsModified = true;
            await _applicationContext.SaveChangesAsync();
        }
        else if (DoesCommentIdExist)
        {
            var parentCommentEntity = _mapper.Map<Comment>(await _commentQueryRepository.GetCommentByIdPlain(commentDTO.CommentId!.Value, c => c.Author));
        
            parentCommentEntity.CommentsIds.Remove(commentDTO.Id);
            
            _applicationContext.Attach(parentCommentEntity);
            _applicationContext.Entry(parentCommentEntity).Property(c => c.CommentsIds).IsModified = true;
            await _applicationContext.SaveChangesAsync();
        }
    }

    public async Task ViewComment(int id, int authorId)
    {   
        var commentDTO = await _commentQueryRepository.GetCommentByIdPlain(id);
        
        if (commentDTO == null)
        {
            throw new CommentNotFoundException();
        }

        var commentEntity = _mapper.Map<Comment>(commentDTO);
        var userEntity = _mapper.Map<User>(await _userQueryRepository.GetUserById(authorId));

        if (commentEntity.ViewedBy.Contains(authorId))
        {
            return;
        }
        
        commentEntity.ViewedBy.Add(authorId);
        
        _applicationContext.Attach(commentEntity);
        
        _applicationContext.Entry(commentEntity).Property(c => c.ViewedBy).IsModified = true;
        
        await _applicationContext.SaveChangesAsync();
    }

    public async Task<IEnumerable<ImageDTO>> GetImagesOfCommentId(int commentId)
    {
        var commentEntity = await _commentQueryRepository.GetCommentByIdPlain(commentId, c => c.Images);

        if (commentEntity == null)
        {
            throw new CommentNotFoundException();
        }
        
        var result = new List<ImageDTO>();
        
        foreach (var image in commentEntity.Images)
        {
            result.Add(_mapper.Map<ImageDTO>(image));
        }

        return result;
    }
}