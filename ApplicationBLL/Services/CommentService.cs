using ApplicationBLL.Exceptions;
using ApplicationBLL.Extentions;
using ApplicationBLL.Services.Abstract;
using ApplicationCommon.DTOs.Comment;
using ApplicationCommon.DTOs.Post;
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
        var commentForDepthChecking = await _applicationContext.Comments.FirstOrDefaultAsync(c => c.Id == currentCheckingId);
        
        if (commentForDepthChecking == null)
        {
            throw new CommentNotFoundException("Comment with specified id not found");
        }

        while (commentForDepthChecking.CommentId != null)
        {
            depth++;
            currentCheckingId = commentForDepthChecking.CommentId.Value;
            commentForDepthChecking = await _applicationContext.Comments.FirstOrDefaultAsync(c => c.Id == currentCheckingId);
        }

        Console.WriteLine(depth);
        
        var comment = await _applicationContext.Comments.AsNoTracking().Include(c => c.Author).CustomInclude(depth).FirstOrDefaultAsync(c => c.Id == id);

        
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

    public async Task PostComment(CommentCreateDTO Comment)
    {
        var authorDTO = await _userService.GetUserById(Comment.UserId);

        bool DoesPostIdExist = Comment.PostId.HasValue;
        bool DoesCommentIdExist = Comment.CommentId.HasValue;

        if (!(DoesCommentIdExist ^ DoesPostIdExist))
        {
            throw new InvalidDataException("Comment is invalid");
        }
        
        var commentDTO = _mapper.Map<CommentDTO>(Comment);
        commentDTO.Author = authorDTO;
        ValidationResult validationResult = await _commentValidator.ValidateAsync(commentDTO);

        if (!validationResult.IsValid)
        {
            throw new ValidationException(validationResult.Errors[0].ErrorMessage);
        }
        
        if (DoesPostIdExist)
        {

            Console.WriteLine("bop");
            
            var postDTO = await _postService.GetPostById(Comment.PostId!.Value);

            commentDTO.Post = postDTO;
            
            InitComment(ref commentDTO);

            var commentEntity = _mapper.Map<Comment>(commentDTO);
            

            if (commentEntity.Author.Id == commentEntity.Post.Author.Id)
            {
                commentEntity.Post.Author = null;
                commentEntity.Post.UserId = commentEntity.Author.Id;
            }
            
            _applicationContext.Attach(commentEntity.Author);
            
            
            _applicationContext.Comments.Add(commentEntity);
            _applicationContext.Attach(commentEntity.Post!);
            await _applicationContext.SaveChangesAsync();

            _applicationContext.Entry(commentEntity.Post!).State = EntityState.Detached;
            _applicationContext.Entry(commentEntity.Post!.Author!).State = EntityState.Detached;

            commentDTO.Id = commentEntity.Id;
            
            postDTO.CommentsIds.Add(commentDTO.Id);
            
            await _postService.PutPost(postDTO.Id, postDTO);
        }
        else if (DoesCommentIdExist)
        {
            var parentCommentDTO = await GetCommentById(Comment.CommentId!.Value);

            commentDTO.ParentComment = parentCommentDTO;
            
            InitComment(ref commentDTO);

            var commentEntity = _mapper.Map<Comment>(commentDTO);
            
            if (commentEntity.Author.Id == commentEntity.ParentComment!.Author.Id)
            {
                commentEntity.ParentComment.Author = null;
                commentEntity.ParentComment.UserId = commentEntity.Author.Id;
            }

            _applicationContext.Attach(commentEntity.ParentComment!);
            _applicationContext.Attach(commentEntity.Author);
            _applicationContext.Comments.Add(commentEntity);
            await _applicationContext.SaveChangesAsync();

            _applicationContext.ChangeTracker.Clear();
            
            commentDTO.Id = commentEntity.Id;
            
            parentCommentDTO.CommentsIds.Add(commentDTO.Id);
            
            await PutComment(parentCommentDTO.Id, parentCommentDTO);
        }
    }

    private static void InitComment(ref CommentDTO commentDto)
    {
        commentDto.CreatedAt = DateTime.UtcNow;
        commentDto.LikesIds = new List<int>();
        commentDto.CommentsIds = new List<int>();
        commentDto.ViewedBy = new List<int>();
    }

    public async Task PutComment(int id, CommentDTO Comment)
    {
        var commentDTO = await GetCommentById(Comment.Id);
        
        ValidationResult validationResult = await _commentValidator.ValidateAsync(commentDTO);

        if (!validationResult.IsValid)
        {
            throw new ValidationException(validationResult.Errors[0].ErrorMessage);
        }

        var commentEntity = _mapper.Map<Comment>(commentDTO);

        commentEntity.TextContent = Comment.TextContent;
        commentEntity.Images = Comment.Images;
        commentEntity.LikesIds = Comment.LikesIds;
        commentEntity.CommentsIds = Comment.CommentsIds;
        commentEntity.ViewedBy = Comment.ViewedBy;

        _applicationContext.Update(commentEntity);
        await _applicationContext.SaveChangesAsync();
    }

    public async Task DeleteComment(int id)
    {
        var commentDTO = await GetCommentById(id);

        var commentEntity = _mapper.Map<Comment>(commentDTO);

        _applicationContext.Comments.Remove(commentEntity);
        await _applicationContext.SaveChangesAsync();
    }
}