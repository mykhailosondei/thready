using System.Runtime.CompilerServices;
using ApplicationBLL.Exceptions;
using ApplicationBLL.Services.Abstract;
using ApplicationCommon.DTOs.Post;
using ApplicationCommon.DTOs.User;
using ApplicationDAL.Context;
using ApplicationDAL.Entities;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using FluentValidation;
using FluentValidation.Results;


namespace ApplicationBLL.Services;

public class PostService : BaseService
{
    private readonly IValidator<PostDTO> _postValidator;
    private readonly UserService _userService;
    
    public PostService(ApplicationContext applicationContext, IMapper mapper, UserService userService, IValidator<PostDTO> postValidator) : base(applicationContext, mapper)
    {
        _userService = userService;
        _postValidator = postValidator;
    }

    
    protected PostService() : base(null, null)
    {
        
    }

    public async Task<IEnumerable<PostDTO>> GetAllPosts()
    {
        var posts = await _applicationContext.Posts.Include(post => post.Author).ToListAsync();
        
        return _mapper.Map<IEnumerable<PostDTO>>(posts);
    }

    public virtual async Task<PostDTO> GetPostById(int id)
    {
        var postModel = await _applicationContext.Posts.FirstOrDefaultAsync(p => p.Id == id);
        if (postModel == null)
        {
            throw new PostNotFoundException();
        }

        return _mapper.Map<PostDTO>(postModel!);
    }
    
    public async Task<IEnumerable<PostDTO>> GetPostsByUserId(int id)
    {
        var userModel = await _userService.GetUserById(id);
        await _applicationContext.Entry(userModel).Collection(user => user.Posts).LoadAsync();
        return userModel.Posts;
        
    }

    public async Task BookmarkPost(int postId, int userId)
    {
        var userModel = await _userService.GetUserById(userId);
        if (await DoesPostExist(postId))
        {
            throw new PostNotFoundException();
        }

        bool isBookmarked = userModel.BookmarkedPostsIds.Contains(postId);

        if (isBookmarked)
            return;
        userModel.BookmarkedPostsIds.Add(postId);
        await _userService.PutUser(userId, userModel);
    }
    
    public async Task RemoveFromBookmarksPost(int postId, int userId)
    {
        var userModel = await _userService.GetUserById(userId);
        if (await DoesPostExist(postId))
        {
            throw new PostNotFoundException();
        }

        bool isBookmarked = userModel.BookmarkedPostsIds.Contains(postId);

        if (!isBookmarked)
            return;
        userModel.BookmarkedPostsIds.Remove(postId);;
        await _userService.PutUser(userId, userModel);
    }

    public async Task Repost(int postId, int userId)
    {
        var userModel = await _userService.GetUserById(userId);
        var post = await GetPostById(postId);

        bool isReposted = userModel.RepostsIds.Contains(postId);

        if (isReposted)
            return;
        userModel.RepostsIds.Add(postId);
        await _userService.PutUser(userId, userModel);
    }
    
    public async Task UndoRepost(int postId, int userId)
    {
        var userModel = await _userService.GetUserById(userId);
        var post = await GetPostById(postId);

        bool isReposted = userModel.RepostsIds.Contains(postId);

        if (!isReposted)
            return;
        userModel.RepostsIds.Remove(postId);
        await _userService.PutUser(userId, userModel);
    }

    public async Task CreatePost(PostCreateDTO post)
    {
        var postEntity = _mapper.Map<Post>(post);
        var postDTO = _mapper.Map<PostDTO>(postEntity); 
        ValidationResult validationResult = await _postValidator.ValidateAsync(postDTO);

        if (!validationResult.IsValid)
        {
            throw new ValidationException(new EmptyPostException().Message);
        }

        postEntity.LikesIds = new List<int>();
        postEntity.ViewedBy = new List<int>();
        postEntity.CommentsIds = new List<int>();
        postEntity.Bookmarks = 0;
        postEntity.RepostersIds = new List<int>();
        _applicationContext.Posts.Add(postEntity);
        _applicationContext.Attach(postEntity.Author);
        await _applicationContext.SaveChangesAsync();
    }

    public virtual async Task PutPost(int id, PostDTO post)
    {
        var postToUpdate = await GetPostById(id);
        ValidationResult validationResult = await _postValidator.ValidateAsync(post);

        if (!validationResult.IsValid)
        {
            throw new ValidationException(new EmptyPostException().Message);
        } 
        postToUpdate.Images = post.Images;
        postToUpdate.TextContent = post.TextContent;
        postToUpdate.CommentsIds = post.CommentsIds;

        _applicationContext.Posts.Update(_mapper.Map<Post>(postToUpdate));
        await _applicationContext.SaveChangesAsync();
    }

    public async Task DeletePost(int id)
    {
        var post = await GetPostById(id);
        _applicationContext.Posts.Remove(_mapper.Map<Post>(post));
        await _applicationContext.SaveChangesAsync();
    }

    private async Task<bool> DoesPostExist(int postId)
    {
        return await _applicationContext.Posts.FirstOrDefaultAsync(p => p.Id == postId) == null;
    }
}