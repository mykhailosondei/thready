using System.Linq.Expressions;
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
using Microsoft.EntityFrameworkCore.ChangeTracking;


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
        var posts = await _applicationContext.Posts.AsNoTracking().Include(post => post.Author).ToListAsync();
        
        return _mapper.Map<IEnumerable<PostDTO>>(posts);
    }

    public virtual async Task<PostDTO> GetPostById(int id, params Expression<Func<Post, object>>[] includeExpressions)
    {
        var query = _applicationContext.Posts.AsNoTracking();

        foreach (var includeExpression in includeExpressions)
        {
            query = query.Include(includeExpression);
        }

        var postModel = await query.FirstOrDefaultAsync(p => p.Id == id);

        if (postModel == null)
        {
            throw new PostNotFoundException();
        }

        return _mapper.Map<PostDTO>(postModel!);
    }

    
    public async Task<IEnumerable<PostDTO>> GetPostsByUserId(int id)
    {
        var userModel = await _userService.GetUserById(id);
        User userEntity = _mapper.Map<User>(userModel);
        
        
        _applicationContext.Attach(userEntity);
        await _applicationContext.Entry(userEntity).Collection(u => u.Posts).LoadAsync();
        
        userModel = _mapper.Map<UserDTO>(userEntity);
        return userModel.Posts;
    }

    public async Task BookmarkPost(int postId, int userId)
    {
        var userModel = await _userService.GetUserById(userId);
        var post = await GetPostById(postId);
        if (await DoesPostExist(postId))
        {
            throw new PostNotFoundException();
        }

        bool isBookmarked = userModel.BookmarkedPostsIds.Contains(postId);

        if (isBookmarked)
            throw new InvalidOperationException("Already bookmarked");
        
        var userEntity = _mapper.Map<User>(userModel);
        var postEntity = _mapper.Map<Post>(post);

        postEntity.Bookmarks++;
        userEntity.BookmarkedPostsIds.Add(postId);
        await BookmarkEntitiesSaveChanges(userEntity, postEntity);
    }
    
    public async Task RemoveFromBookmarksPost(int postId, int userId)
    {
        var userModel = await _userService.GetUserById(userId);
        var post = await GetPostById(postId);
        if (await DoesPostExist(postId))
        {
            throw new PostNotFoundException();
        }

        bool isBookmarked = userModel.BookmarkedPostsIds.Contains(postId);

        if (!isBookmarked)
        {
            throw new InvalidOperationException("Not bookmarked");
        }

        var userEntity = _mapper.Map<User>(userModel);
        var postEntity = _mapper.Map<Post>(post);

        postEntity.Bookmarks--;
        userEntity.BookmarkedPostsIds.Remove(postId);
        await BookmarkEntitiesSaveChanges(userEntity, postEntity);
    }

    public async Task Repost(int postId, int userId)
    {
        var userModel = await _userService.GetUserById(userId);
        var post = await GetPostById(postId);

        bool isReposted = userModel.RepostsIds.Contains(postId);

        if (isReposted)
        {
            throw new InvalidOperationException("Already reposted");
        }

        var userEntity = _mapper.Map<User>(userModel);
        var postEntity = _mapper.Map<Post>(post);
        
        userEntity.RepostsIds.Add(postId);
        postEntity.RepostersIds.Add(userId);

        await RepostEntitiesSaveChanges(userEntity, postEntity);
    }

    private async Task BookmarkEntitiesSaveChanges(User userEntity, Post postEntity)
    {
        _applicationContext.Attach(userEntity);
        _applicationContext.Attach(postEntity);
        _applicationContext.Entry(userEntity).Property(u => u.BookmarkedPostsIds).IsModified = true;
        _applicationContext.Entry(postEntity).Property(p => p.Bookmarks).IsModified = true;
        await _applicationContext.SaveChangesAsync();
    }

    private async Task RepostEntitiesSaveChanges(User userEntity, Post postEntity)
    {
        _applicationContext.Attach(userEntity);
        _applicationContext.Attach(postEntity);

        _applicationContext.Entry(postEntity).Property(p => p.RepostersIds).IsModified = true;
        _applicationContext.Entry(userEntity).Property(u => u.RepostsIds).IsModified = true;
        await _applicationContext.SaveChangesAsync();
    }
    
    public async Task UndoRepost(int postId, int userId)
    {
        var userModel = await _userService.GetUserById(userId);
        var post = await GetPostById(postId);

        bool isReposted = userModel.RepostsIds.Contains(postId);

        if (!isReposted)
        {
            throw new InvalidOperationException("Not reposted");
        }
        
        var userEntity = _mapper.Map<User>(userModel);
        var postEntity = _mapper.Map<Post>(post);
        
        userEntity.RepostsIds.Remove(postId);
        postEntity.RepostersIds.Remove(userId);

        await RepostEntitiesSaveChanges(userEntity, postEntity);
    }

    public async Task CreatePost(PostCreateDTO post)
    {
        var postEntity = _mapper.Map<Post>(post);
        var postDTO = _mapper.Map<PostDTO>(postEntity); 
        ValidationResult validationResult = await _postValidator.ValidateAsync(postDTO);

        if (!validationResult.IsValid)
        {
            throw new EmptyPostException();
        }

        InitPost(ref postEntity);
        
        _applicationContext.Posts.Add(postEntity);
        _applicationContext.Attach(postEntity.Author);
        await _applicationContext.SaveChangesAsync();
    }

    private void InitPost(ref Post postEntity)
    {
        postEntity.CreatedAt = DateTime.UtcNow;
        postEntity.LikesIds = new List<int>();
        postEntity.ViewedBy = new List<int>();
        postEntity.CommentsIds = new List<int>();
        postEntity.Bookmarks = 0;
        postEntity.RepostersIds = new List<int>();
    }

    private Func<Image, string> imageSelector = image => image.Url;

    public virtual async Task PutPost(int id, PostUpdateDTO post)
    {
        var postToUpdate = await GetPostById(id);
        
        ValidationResult validationResult = await _postValidator.ValidateAsync(postToUpdate);

        if (!validationResult.IsValid)
        {
            throw new EmptyPostException();
        }

        var postEntity = _mapper.Map<Post>(postToUpdate);
        var updatedImages = post.Images.Select(i => _mapper.Map<Image>(i)).ToList();

        _applicationContext.Attach(postEntity);

        await _applicationContext.Entry(postEntity).Collection(p => p.Images).LoadAsync();

        var imagesToAdd = updatedImages.ExceptBy(postEntity.Images.Select(imageSelector), imageSelector).ToList();
        var imagesToDelete = postEntity.Images.ExceptBy(updatedImages.Select(imageSelector), imageSelector).ToList();
        
        postEntity.Images.AddRange(imagesToAdd.DistinctBy(imageSelector));

        foreach (var image in imagesToDelete)
        {
            postEntity.Images.RemoveAll(i => i.Url == image.Url);
            _applicationContext.Entry(image).State = EntityState.Deleted;
        }
        
        postEntity.TextContent = post.TextContent;

        _applicationContext.Entry(postEntity).Collection(p => p.Images).IsModified = true;
        _applicationContext.Entry(postEntity).Property(p => p.TextContent).IsModified = true;
        await _applicationContext.SaveChangesAsync();
    }

    public async Task DeletePost(int postId)
    {
        var post = await GetPostById(postId);
        
        _applicationContext.Posts.Remove(_mapper.Map<Post>(post));
        
        await _applicationContext.SaveChangesAsync();
        
        var reposterUsers = post.RepostersIds.Select(async i => _mapper.Map<User>(await _userService.GetUserById(i)));
        
        foreach (var reposterUser in reposterUsers)
        {
            var awaitedUser = await reposterUser;
            awaitedUser.RepostsIds.Remove(postId);
            _applicationContext.Attach(awaitedUser);
            _applicationContext.Entry(awaitedUser).Property(u => u.RepostsIds).IsModified = true;
        }
        await _applicationContext.SaveChangesAsync();
    }

    private async Task<bool> DoesPostExist(int postId)
    {
        return await _applicationContext.Posts.FirstOrDefaultAsync(p => p.Id == postId) == null;
    }
}